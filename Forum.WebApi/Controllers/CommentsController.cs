using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Services.CommentsManagement;

namespace Forum.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService articleService)
        {
            _commentService = articleService;
        }

        [HttpGet]
        [Route("{articleId:guid:length(36)}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetComments(string articleId)
        {
            var articles = await _commentService.GetCommentsByArticleId(articleId);
            return Ok(articles);
        }

        [HttpGet]
        [Route("{articleId:guid:length(36)}/{commentId:guid:length(36)}")]
        public async Task<ActionResult<Comment>> GetComment(string commentId)
        {
            var comment = await _commentService.GetEntityAsync(commentId);
            return Ok(comment);
        }

        [HttpPost]
        [Route("{articleId:guid:length(36)}")]
        public async Task<ActionResult<Comment>> AddComment([FromBody] CommentRequest commentRequest, string articleId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentService.CreateEntityAsync(commentRequest);
            if (comment == null)
                return BadRequest();

            var uri = $"/articles/{articleId}/{comment.Id}";
            return Created(uri, comment);
        }

        [HttpPut]
        [Route("{articleId:guid:length(36)}/{commentId:guid:length(36)}")]
        public async Task<ActionResult> UpdateComment(string commentId, [FromBody] CommentRequest articleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _commentService.UpdateEntityAsync(commentId, articleRequest);
            return NoContent();
        }

        [HttpDelete]
        [Route("{articleId:guid:length(36)}/{commentId:guid:length(36)}")]
        public async Task<ActionResult> DeleteComment(string commentId)
        {
            await _commentService.DeleteEntityAsync(commentId);
            return Ok();
        }
    }
}