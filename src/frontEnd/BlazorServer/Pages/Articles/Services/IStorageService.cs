using System.IO;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles.Services
{
    public interface IStorageService
    {
        Task<string> UploadAsync(string filename, int id, Stream file);
    }
}