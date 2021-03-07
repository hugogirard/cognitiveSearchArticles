using Article.Api.Models;
using Article.Api.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleContext _context;
        private readonly IEventPublisher _eventPublisher;

        public ArticleController(ArticleContext context,IEventPublisher eventPublisher)
        {
            _context = context;
            _eventPublisher = eventPublisher;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<Models.Article>),200)]
        public async Task<IActionResult> All() 
        {
            var articles = await _context.Articles.ToListAsync();

            return Ok(articles);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Models.Article), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id) 
        {
            var article = await _context.Articles.SingleOrDefaultAsync(a => a.Id == id);

            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Models.Article), 202)]
        public async Task<IActionResult> Create([FromBody] Models.Article article) 
        {
            var newArticle = new Models.Article()
            {
                Title = article.Title,
                Text = article.Text
            };

            await _context.Articles.AddAsync(newArticle);
            
            await _context.SaveChangesAsync();

            await _eventPublisher.SendEventAsync(newArticle.Id, newArticle.Title);

            return Ok(newArticle);
        }
    }
}
