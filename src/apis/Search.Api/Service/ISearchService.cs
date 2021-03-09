using Search.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Search.Api.Service
{
    public interface ISearchService
    {
        Task<IEnumerable<Article>> RunQueryAsync(string query);
    }
}