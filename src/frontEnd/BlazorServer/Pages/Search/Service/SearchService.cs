using BlazorServer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Search.Service
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;

        public SearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Article>> SearchAsync(string query)
        {
            dynamic parameters = new ExpandoObject();
            parameters.SearchQuery = query;

            var serializedObject = JsonSerializer.Serialize(parameters);
            var jsoncontent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await this._httpClient.PostAsync(string.Empty, jsoncontent);

            var results = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<Article>>(results);
        }
    }
}
