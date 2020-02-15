using Forum.Models.CommentsManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Services.CommentsManagement
{
    public interface ICommentService : IEntityService<Comment, CommentRequest>
    {
        Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid articleId);
    }
}
