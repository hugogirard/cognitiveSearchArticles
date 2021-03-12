using Article.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ArticleContext _context;

        public CategoryController(ArticleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Models.Category>), 200)]
        public async Task<IActionResult> Get() 
        {
            var categories = await _context.Categories.ToListAsync();
            
            return Ok(categories.Select(e => new { e.Id, e.Name }));
        }
    }
}
