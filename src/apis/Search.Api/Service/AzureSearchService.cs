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
using Search.Api.Models;

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
            SearchResults<Article> response;
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

            response = await _srchclient.SearchAsync<Article>(parameters.SearchQuery, options);

            if (response.TotalCount > 0) 
            {
                if (response.Facets.Any()) 
                { 
                    articleResult.Facets = response.Facets.First();
                }

                articleResult.Articles = response.GetResults().Select(e => e.Document);
            }

            return articleResult;
        }
    }
}
