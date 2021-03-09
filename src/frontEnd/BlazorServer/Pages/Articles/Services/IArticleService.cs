using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles.Services
{
    public interface IArticleService
    {
        Task<bool> CreateArticleAsync(NewArticleViewModel vm);
    }
}