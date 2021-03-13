using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorServer.Infrastructure
{
    public abstract class BaseService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _options;

        public BaseService(HttpClient client)
        {
            _http = client;

            _options = new JsonSerializerOptions();
            _options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        public virtual async Task<Y> PostAsync<T, Y>(T content,string url) where T : class where Y : class
        {
            var serializedObject = JsonSerializer.Serialize(content, _options);
            var jsoncontent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await this._http.PostAsync(url, jsoncontent);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Y>(jsonResponse, _options);
        }

        public virtual async Task<Y> PostAsync<Y>(dynamic content, string url) where Y: class
        {
            var serializedObject = JsonSerializer.Serialize(content, _options);
            var jsoncontent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await this._http.PostAsync(url, jsoncontent);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Y>(jsonResponse, _options);
        }

        public virtual async Task<Y> GetAsync<Y>(string url) where Y : class 
        {
            var response = await this._http.GetAsync(url);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Y>(jsonResponse, _options);

        }
    }
}
