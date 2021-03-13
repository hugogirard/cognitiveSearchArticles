using BlazorServer.Infrastructure;
using BlazorServer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Article.Shared;

namespace BlazorServer.Pages.Search.Service
{
    public class SearchService : BaseService, ISearchService
    {
        public SearchService(HttpClient client) : base(client)
        {
        }

        public async Task<ArticleResult> SearchAsync(string query,string category)
        {
            dynamic parameters = new ExpandoObject();
            parameters.SearchQuery = query;

            if (!string.IsNullOrEmpty(category))
                parameters.Category = category;

            return await base.PostAsync<ArticleResult>((object)parameters,string.Empty);
        }

    }
}
