using BlazorServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Search.Service
{
    public interface ISearchService
    {
        Task<IEnumerable<Article>> SearchAsync(string query);
    }
}