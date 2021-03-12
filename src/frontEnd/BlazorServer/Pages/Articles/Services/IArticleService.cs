using BlazorServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles.Services
{
    public interface IArticleService
    {
        Task<Article> CreateArticleAsync(NewArticleViewModel vm);
        Task<IEnumerable<CategoryList>> GetCategoriesAsync();
    }
}