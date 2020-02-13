using Forum.Models.CommentsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Dal.Repository;
using Forum.Models.ArticlesManagement;

namespace Forum.Services.CommentsManagement
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;
        private readonly IMapper _mapper;

        public CommentService(IRepository<Comment> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Comment> CreateEntityAsync(CommentRequest commentRequest)
        {
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            if (comment.Content.Length >= 200)
            {
                return null;
            }
            await _repository.Add(comment);
            return comment;
        }

        public async Task DeleteEntityAsync(string entityId)
        {
            await _repository.Remove(entityId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(string articleId)
        {
            var comments = await _repository.GetAll();
            return comments.Where(c => c.ArticleId == new Guid(articleId)).ToList();
        }

        public async Task<IEnumerable<Comment>> GetEntitiesAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Comment> GetEntityAsync(string entityId)
        {
            return await _repository.Find(entityId);
        }

        public async Task<Comment> UpdateEntityAsync(string entityId, CommentRequest entityRequest)
        {
            var comment = _mapper.Map<CommentRequest, Comment>(entityRequest);
            if (comment.Content.Length >= 200)
            {
                return null;
            }
            await _repository.Update(comment);
            return comment;
        }
    }
}
