using Models = BlazorServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles.Services
{
    public interface IArticleService
    {
        Task<Models.Article> CreateArticleAsync(NewArticleViewModel vm);
        Task<IEnumerable<Models.CategoryList>> GetCategoriesAsync();
    }
}