using BlazorServer.Models;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles.Services
{
    public interface IArticleService
    {
        Task<Article> CreateArticleAsync(NewArticleViewModel vm);
    }
}