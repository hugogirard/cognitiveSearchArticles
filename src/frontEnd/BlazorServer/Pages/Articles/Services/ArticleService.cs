using BlazorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using BlazorServer.Infrastructure;

namespace BlazorServer.Pages.Articles.Services
{
    public class ArticleService : BaseService, IArticleService
    {
        public ArticleService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<Article> CreateArticleAsync(NewArticleViewModel vm)
        {
            var article = new Article
            { 
                Title = vm.Title,
                ShortDescription = vm.ShortDescription,
                Text = vm.Content,
                CategoryId =  int.Parse(vm.CategoryId)
            };

            return await base.PostAsync<Article,Article>(article,"article");
        }

        public async Task<IEnumerable<CategoryList>> GetCategoriesAsync()
        {
            return await base.GetAsync<IEnumerable<CategoryList>>("category");
        }
    }
}
