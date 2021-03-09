using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Search.Documents.Indexes;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using System.Collections.Generic;

namespace ArticleFunction
{
    public class Indexer
    {
        private static SearchIndexClient _indexClient;
        public SearchIndexClient IndexClient
        {
            get 
            {
                if (_indexClient == null) 
                {
                    _indexClient = new SearchIndexClient(new Uri(Environment.GetEnvironmentVariable("SearchServiceUrl")),
                                                         new AzureKeyCredential(Environment.GetEnvironmentVariable("SearchServiceAdminApiKey")));
                }

                return _indexClient;
            }
        }

        private SearchIndexerClient _searchIndexerClient;
        public SearchIndexerClient SearchIndexerClient 
        {
            get 
            { 
                if (_searchIndexerClient == null)
                { 
                    _searchIndexerClient = new SearchIndexerClient(new Uri(Environment.GetEnvironmentVariable("SearchServiceUrl")), 
                                                                   new AzureKeyCredential(Environment.GetEnvironmentVariable("SearchServiceAdminApiKey")));
                }

                return _searchIndexerClient;
            }
        }

        [FunctionName("Index")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                
                
                await RunSqlDbIndexer(log);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message,ex);
            }

            return new OkObjectResult("YEAH");
        }

        private async Task RunSqlDbIndexer(ILogger log) 
        {
            var sqlDataSource = new SearchIndexerDataSourceConnection(name: "sqldatasource",
                                                                      type: SearchIndexerDataSourceType.AzureSql,
                                                                      connectionString: Environment.GetEnvironmentVariable("SqlConnectionString"),
                                                                      container: new SearchIndexerDataContainer("Articles"));

            await this.SearchIndexerClient.CreateOrUpdateDataSourceConnectionAsync(sqlDataSource);

            SearchIndexer sqlIndexer = new SearchIndexer(name: "article-sql-indexer",
                                                         dataSourceName: sqlDataSource.Name,
                                                         targetIndexName: "article")
            {
                Schedule = new IndexingSchedule(TimeSpan.FromMinutes(60))
            };

            try
            {
                sqlIndexer.FieldMappings.Add(new FieldMapping("Id")
                {
                    TargetFieldName = "id"
                });

                await this.SearchIndexerClient.GetIndexerAsync(sqlIndexer.Name);

                // UNCOMMENT THIS LINE TO RESET THE INDEXER
                await this.SearchIndexerClient.ResetIndexerAsync(sqlIndexer.Name);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Is possible you get a 404 when the index doesn't exists
      
            }

            await this.SearchIndexerClient.CreateOrUpdateIndexerAsync(sqlIndexer);

            try
            {
                await this.SearchIndexerClient.RunIndexerAsync(sqlIndexer.Name);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }
    }
}
