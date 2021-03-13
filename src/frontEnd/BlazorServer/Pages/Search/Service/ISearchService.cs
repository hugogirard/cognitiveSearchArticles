using BlazorServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Search.Service
{
    public interface ISearchService
    {
        Task<ArticleResult> SearchAsync(string query, string category);
    }
}