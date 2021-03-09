using Microsoft.AspNetCore.Mvc;
using Search.Api.Models;
using Search.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]        
        public async Task<IActionResult> GetAsync([FromBody] SearchParameter searchParameter) 
        {
            var results = await _searchService.RunQueryAsync(searchParameter.SearchQuery);

            return new OkObjectResult(results);
        }
    }
}
