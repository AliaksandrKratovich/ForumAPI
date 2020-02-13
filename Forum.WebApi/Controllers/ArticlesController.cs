using Forum.Models.ArticlesManagement;
using Forum.Services.ArticlesManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _articleService.GetEntitiesAsync();
            return Ok(articles);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<Article>>> GetSortedArticles([FromBody]FilterArticle filter)
        {
            var articles = await _articleService.GetArticlesByOccurrenceAsync(filter.Title, filter.UserName, filter.Category);
            return Ok(articles);
        }

        [HttpGet]
        [Route("{articleId:guid:length(36)}")]
        public async Task<ActionResult<Article>> GetArticle(string articleId)
        {
            var article = await _articleService.GetEntityAsync(articleId);
            
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Article>> AddArticle([FromBody] ArticleRequest articleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var article = await _articleService.CreateEntityAsync(articleRequest);
            if (article == null)
                return BadRequest();

            var uri = $"/articles/{article.Id}";
            return Created(uri, article);
        }

        [HttpPut]
        [Route("{articleId:guid:length(36)}")]
        public async Task<ActionResult> UpdateArticle(string articleId, [FromBody] ArticleRequest articleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.UpdateEntityAsync(articleId, articleRequest);
            return NoContent();
        }

        [HttpDelete]
        [Route("{articleId:guid:length(36)}")]
        public async Task<ActionResult> DeleteArticle(string articleId)
        {
            await _articleService.DeleteEntityAsync(articleId);
            return Ok();
        }
    }
}