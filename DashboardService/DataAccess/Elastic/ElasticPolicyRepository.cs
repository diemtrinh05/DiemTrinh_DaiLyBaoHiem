using System;
using System.Linq;
using DashboardService.Domain;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;

namespace DashboardService.DataAccess.Elastic;

public class ElasticPolicyRepository : IPolicyRepository
{
    private readonly ElasticsearchClient elasticClient;
    private readonly ILogger<ElasticPolicyRepository> logger;

    public ElasticPolicyRepository(ElasticsearchClient elasticClient, ILogger<ElasticPolicyRepository> logger)
    {
        this.elasticClient = elasticClient;
        this.logger = logger;
    }

    public void Save(PolicyDocument policy)
    {
        var response = elasticClient.Index
        (
            policy,
            i => i
                .Index("policy_lab_stats")
                .Id(policy.Number)
                .Refresh(Refresh.True)
        );

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to index policy document {PolicyNumber} into Elasticsearch. Response: {Response}",
                policy.Number, response.ElasticsearchServerError?.ToString() ?? response.DebugInformation);
        }
    }

    public PolicyDocument FindByNumber(string policyNumber)
    {
        var searchResponse = elasticClient.Search<PolicyDocument>
        (
            s => s
                .Query(q => q
                    .Bool(b => b
                        .Filter(bf => bf
                            .Term(
                                 p => new Field("number.keyword"), policyNumber)))
                )
        );

        return searchResponse.Documents.FirstOrDefault();
    }

    public AgentSalesQueryResult GetAgentSales(AgentSalesQuery query)
    {
        var adapter = AgentSalesQueryAdapter.For(query);
        var response = elasticClient.Search<PolicyDocument>(adapter.BuildQuery());
        return adapter.ExtractResult(response);
    }

    public TotalSalesQueryResult GetTotalSales(TotalSalesQuery query)
    {
        var adapter = TotalSalesQueryAdapter.For(query);
        var response = elasticClient.Search<PolicyDocument>(adapter.BuildQuery());
        return adapter.ExtractResult(response);
    }

    public SalesTrendsResult GetSalesTrend(SalesTrendsQuery query)
    {
        var adapter = SalesTrendsQueryAdapter.For(query);
        var response = elasticClient.Search<PolicyDocument>(adapter.BuildQuery());
        return adapter.ExtractResult(response);
    }
}
