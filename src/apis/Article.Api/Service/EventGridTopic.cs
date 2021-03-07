using Article.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Article.Api.Service
{
    public class EventGridTopic : IEventPublisher
    {
        private readonly ILogger<EventGridTopic> _logger;
        private readonly HttpClient _http;

        public EventGridTopic(HttpClient httpClient, IConfiguration configuration, ILogger<EventGridTopic> logger)
        {
            httpClient.BaseAddress = new Uri(configuration["EventGridTopics"]);
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", configuration["EventGridKey"]);
            _logger = logger;

            _http = httpClient;
        }

        public async Task<bool> SendEventAsync(int articleId, string title)
        {
            var @event = new NewArticleEvent(articleId, title);

            var events = new List<NewArticleEvent> { @event };

            var serializedItem = new StringContent(JsonSerializer.Serialize(events), Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(string.Empty, serializedItem);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogError("Cannot send event new article", response.StatusCode);
                return true;
            }

            return false;
        }
    }
}
