using Forum.Models.ArticlesManagement;
using Forum.Services.ArticlesManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _articleService.GetEntitiesAsync();
            return Ok(articles);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Article>>> GetSortedArticles(string title, string userName, string category)
        {
            var articles = await _articleService.GetArticlesByOccurrenceAsync(title, userName, category);
            return Ok(articles);
        }

        [AllowAnonymous]
        [HttpGet("{articleId:guid}")]
        public async Task<ActionResult<Article>> GetArticle(Guid articleId)
        {
            var article = await _articleService.GetEntityAsync(articleId);

            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult<Article>> AddArticle([FromBody] ArticleRequest articleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            articleRequest.UserName = HttpContext.User.Identity.Name;
            var article = await _articleService.CreateEntityAsync(articleRequest);

            var uri = $"/articles/{article.Id}";
            return Created(uri, article);
        }

        [HttpPut("{articleId:guid}")]
        public async Task<ActionResult> UpdateArticle(Guid articleId, [FromBody] ArticleRequest articleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            articleRequest.UserName = HttpContext.User.Identity.Name;
            await _articleService.UpdateEntityAsync(articleId, articleRequest);
           
            return NoContent();
        }
        
        [HttpDelete("{articleId:guid}")]
        public async Task<ActionResult> DeleteArticle(Guid articleId)
        {
            await _articleService.DeleteEntityAsync(articleId);

            return Ok();
        }
    }
}