using BlazorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace BlazorServer.Pages.Articles.Services
{
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;

        public ArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Article> CreateArticleAsync(NewArticleViewModel vm)
        {
            var article = new Article
            { 
                Title = vm.Title,
                ShortDescription = vm.ShortDescription,
                Text = vm.Content
            };

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            var serializedObject = JsonSerializer.Serialize(article,options);
            var jsoncontent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await this._httpClient.PostAsync(string.Empty, jsoncontent);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Article>(jsonResponse);
        }
    }
}
