using System.Threading.Tasks;

namespace Article.Api.Service
{
    public interface IEventPublisher
    {
        Task<bool> SendEventAsync(int articleId, string title);
    }
}