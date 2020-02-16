using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Forum.Services.CommentsManagement;
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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService articleService)
        {
            _commentService = articleService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetComments(Guid articleId)
        {
            var articles = await _commentService.GetCommentsByArticleId(articleId);
            return Ok(articles);
        }

        [AllowAnonymous]
        [HttpGet("{commentId:guid}")]
        public async Task<ActionResult<Comment>> GetComment(Guid commentId)
        {
            var comment = await _commentService.GetEntityAsync(commentId);

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment([FromBody] CommentRequest commentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            commentRequest.UserName = HttpContext.User.Identity.Name;
            var comment = await _commentService.CreateEntityAsync(commentRequest);

            var uri = $"/articles/{comment.Id}";
            return Created(uri, comment);
        }

        [HttpPut("{commentId:guid}")]
        public async Task<ActionResult> UpdateComment(Guid commentId, [FromBody] CommentRequest commentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            commentRequest.UserName = HttpContext.User.Identity.Name;
            await _commentService.UpdateEntityAsync(commentId, commentRequest);

            return NoContent();
        }

        [HttpDelete("{commentId:guid}")]
        public async Task<ActionResult> DeleteComment(Guid commentId)
        {
            await _commentService.DeleteEntityAsync(commentId);

            return Ok();
        }
    }
}