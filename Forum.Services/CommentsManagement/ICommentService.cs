using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Forum.Models.CommentsManagement;

namespace Forum.Services.CommentsManagement
{
    public interface ICommentService : IEntityService<Comment, CommentRequest>
    {
        Task<IEnumerable<Comment>> GetCommentsByArticleId(string articleId);
    }
}
