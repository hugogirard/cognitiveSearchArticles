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
using ArticleFunction.Models;

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
                await CreateIndex(log);

                await RunSqlDbIndexer(log);

                await RunStorageIndexer(log);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message,ex);
            }

            return new OkObjectResult("YEAH");
        }

        private async Task CreateIndex(ILogger log)
        {
            try
            {
                await this.IndexClient.GetIndexAsync("article");
                await this.IndexClient.DeleteIndexAsync("article");
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {

            }

            FieldBuilder bulder = new FieldBuilder();
            var definition = new SearchIndex("article", bulder.Build(typeof(Article)));

            await this.IndexClient.CreateIndexAsync(definition);
        }

        private async Task RunStorageIndexer(ILogger log)
        {
            var blobDataSource = new SearchIndexerDataSourceConnection(name: "storagedatasource",
                                                                       type: SearchIndexerDataSourceType.AzureBlob,
                                                                       connectionString: Environment.GetEnvironmentVariable("BlobStorageConnectionString"),
                                                                       container: new SearchIndexerDataContainer("files"));

            await this.SearchIndexerClient.CreateOrUpdateDataSourceConnectionAsync(blobDataSource);

            SearchIndexer storageIndexer = new SearchIndexer(name: "storate-indexer",
                                                            dataSourceName: blobDataSource.Name,
                                                            targetIndexName: "article")
            {
                Schedule = new IndexingSchedule(TimeSpan.FromMinutes(60))
            };

            storageIndexer.SkillsetName = "azureblob-skillset";
            storageIndexer.FieldMappings.Add(new FieldMapping("articleId") { TargetFieldName = "Id" });
            storageIndexer.Parameters = new IndexingParameters
            {
                IndexingParametersConfiguration = new IndexingParametersConfiguration
                {
                    DataToExtract = BlobIndexerDataToExtract.ContentAndMetadata,
                    ParsingMode = BlobIndexerParsingMode.Default
                }
            };
                
                
            //    .IndexingParametersConfiguration = new IndexingParametersConfiguration
            //{
            //    DataToExtract = BlobIndexerDataToExtract.ContentAndMetadata                
            //};
            storageIndexer.OutputFieldMappings.Add(new FieldMapping("/document/content/keyphrases") { TargetFieldName= "Keyphrases" });
            try
            {
                await this.SearchIndexerClient.GetIndexerAsync(storageIndexer.Name);

                // UNCOMMENT THIS LINE TO RESET THE INDEXER
                await this.SearchIndexerClient.ResetIndexerAsync(storageIndexer.Name);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Is possible you get a 404 when the index doesn't exists
            }

            await this.SearchIndexerClient.CreateOrUpdateIndexerAsync(storageIndexer);

            try
            {
                await this.SearchIndexerClient.RunIndexerAsync(storageIndexer.Name);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private async Task RunSqlDbIndexer(ILogger log) 
        {
            var sqlDataSource = new SearchIndexerDataSourceConnection(name: "sqldatasource",
                                                                      type: SearchIndexerDataSourceType.AzureSql,
                                                                      connectionString: Environment.GetEnvironmentVariable("SqlConnectionString"),
                                                                      container: new SearchIndexerDataContainer("Articles") 
                                                                      { 
                                                                        Query = @"SELECT a.*,c.Name as Category  
                                                                                    FROM [dbo].[Articles] a
                                                                                         JOIN [dbo].[Categories] c        
                                                                                         ON a.CategoryId = c.Id"
                                                                      });

            await this.SearchIndexerClient.CreateOrUpdateDataSourceConnectionAsync(sqlDataSource);

            SearchIndexer sqlIndexer = new SearchIndexer(name: "article-sql-indexer",
                                                         dataSourceName: sqlDataSource.Name,
                                                         targetIndexName: "article")
            {
                Schedule = new IndexingSchedule(TimeSpan.FromMinutes(60))
            };

            try
            {
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
