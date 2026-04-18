using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolicySearchService.Domain;
using PolicyService.Api.Queries;

namespace PolicySearchService.DataAccess.ElasticSearch;

public class PolicyRepository : IPolicyRepository
{
    private static readonly Regex PolicyNumberPattern = new("^[0-9a-fA-F\\-]{20,}$", RegexOptions.Compiled);
    private readonly ElasticsearchClient elasticClient;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<PolicyRepository> logger;
    private readonly string policyServiceBaseUrl;

    public PolicyRepository(
        ElasticsearchClient elasticClient,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<PolicyRepository> logger)
    {
        this.elasticClient = elasticClient;
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
        policyServiceBaseUrl = configuration["PolicyServiceBaseUrl"] ?? "http://localhost:5050/api";
    }

    public async Task Add(Policy policy)
    {
        var response = await elasticClient.IndexAsync(policy, i => i
            .Index("lab_policies")
            .Id(policy.PolicyNumber)
            .Refresh(Refresh.True));

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to index policy {PolicyNumber} in PolicySearchService. Response: {Response}",
                policy.PolicyNumber, response.ElasticsearchServerError?.ToString() ?? response.DebugInformation);
        }
    }

    public async Task<List<Policy>> Find(string queryText)
    {
        if (LooksLikePolicyNumber(queryText))
            return await FindByPolicyNumberFallback(queryText);

        try
        {
            var result = await elasticClient
                .SearchAsync<Policy>(
                    s =>
                        s.From(0)
                            .Size(10)
                            .Query(q =>
                                q.MultiMatch(mm =>
                                    mm.Query(queryText)
                                        .Fields(Infer.Fields<Policy>(p => p.PolicyNumber, p => p.PolicyHolder))
                                        .Type(TextQueryType.BestFields)
                                        .Fuzziness(new Fuzziness("AUTO"))
                                )
                            ));

            if (result == null)
            {
                logger.LogWarning("Policy search returned null response for query '{QueryText}'.", queryText);
                return new List<Policy>();
            }

            if (!result.IsValidResponse)
            {
                logger.LogError("Failed to search policies in PolicySearchService. Response: {Response}",
                    result.ElasticsearchServerError?.ToString() ?? result.DebugInformation);
                return await FindByPolicyNumberFallback(queryText);
            }

            var policies = result.Documents?.ToList() ?? new List<Policy>();
            if (policies.Count > 0)
                return policies;

            return await FindByPolicyNumberFallback(queryText);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while searching policies for query '{QueryText}'.", queryText);
            return await FindByPolicyNumberFallback(queryText);
        }
    }

    private async Task<List<Policy>> FindByPolicyNumberFallback(string queryText)
    {
        if (string.IsNullOrWhiteSpace(queryText) || queryText.Contains("AND"))
            return new List<Policy>();

        try
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{policyServiceBaseUrl}/policy/{WebUtility.UrlEncode(queryText)}");
            if (!response.IsSuccessStatusCode)
                return new List<Policy>();

            var result = await response.Content.ReadFromJsonAsync<GetPolicyDetailsQueryResult>();
            if (result?.Policy == null)
                return new List<Policy>();

            return new List<Policy>
            {
                new()
                {
                    PolicyNumber = result.Policy.Number,
                    PolicyStartDate = result.Policy.DateFrom,
                    PolicyEndDate = result.Policy.DateTo,
                    ProductCode = result.Policy.ProductCode,
                    PolicyHolder = result.Policy.PolicyHolder,
                    PremiumAmount = result.Policy.TotalPremium
                }
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Fallback policy lookup failed for query '{QueryText}'.", queryText);
            return new List<Policy>();
        }
    }

    private static bool LooksLikePolicyNumber(string queryText)
    {
        if (string.IsNullOrWhiteSpace(queryText))
            return false;

        return PolicyNumberPattern.IsMatch(queryText.Trim());
    }
}
