using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Article.Shared;
using Article.Shared;

namespace Search.Api.Service
{
    public class AzureSearchService : ISearchService
    {
        private readonly SearchClient _srchclient;

        public AzureSearchService(IConfiguration configuration)
        {
            var serviceEndpoint = new Uri(configuration["Search:Endpoint"]);
            var credential = new AzureKeyCredential(configuration["Search:ApiKey"]);

            _srchclient = new SearchClient(serviceEndpoint, configuration["Search:IndexName"], credential);
        }

        public async Task<ArticleResult> RunQueryAsync(SearchParameter parameters)
        {
            SearchResults<Article.Shared.ArticleIndex> response;
            SearchOptions options = new SearchOptions
            {
                IncludeTotalCount = true,
                SearchMode = SearchMode.Any                
            };

            options.Facets.Add("Category");

            if (!string.IsNullOrEmpty(parameters.Category))
            {
                options.Filter = $"Category eq '{parameters.Category}'";
            }

            var articleResult = new ArticleResult();

            response = await _srchclient.SearchAsync<Article.Shared.ArticleIndex>(parameters.SearchQuery, options);

            if (response.TotalCount > 0) 
            {
                if (response.Facets.Any()) 
                { 
                    var facets = response.Facets.First();

                    articleResult.Facets.Key = facets.Key;
                    articleResult.Facets.Value = facets.Value.Select(e => new KeyValue 
                    { 
                        Value = e.Value.ToString(),
                        Count = e.Count
                    });
                }

                articleResult.Articles = response.GetResults().Select(e => e.Document);
            }

            return articleResult;
        }
    }
}
