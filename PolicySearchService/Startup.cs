using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicySearchService.DataAccess.ElasticSearch;
using PolicySearchService.Messaging.RabbitMq;
using PolicyService.Api.Events;
using Steeltoe.Discovery.Client;

namespace PolicySearchService;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDiscoveryClient(Configuration);
        services.AddCors(options =>
            options.AddPolicy("WebClient", builder =>
                builder
                    .WithOrigins("http://localhost:8080", "http://127.0.0.1:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));
        services.AddMvc()
            .AddNewtonsoftJson();
        services.AddMediatR(opts => opts.RegisterServicesFromAssemblyContaining<Startup>());
        services.AddHttpClient();
        services.AddElasticSearch(Configuration.GetConnectionString("ElasticSearchConnection"));
        services.AddRabbitListeners();
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseHsts();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        if (!env.IsDevelopment())
            app.UseHttpsRedirection();
        app.UseCors("WebClient");
        app.UseRabbitListeners(new List<Type> { typeof(PolicyCreated) });
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
