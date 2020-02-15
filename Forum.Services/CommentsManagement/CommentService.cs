using AutoMapper;
using Forum.Dal.Repository;
using Forum.Models.CommentsManagement;
using Forum.WebApi.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                throw new ResponseException("comments bad content length", 400);
            }

            await _repository.Add(comment);
            return comment;
        }

        public async Task<bool> DeleteEntityAsync(Guid entityId)
        {
            if ((await _repository.GetAll()).All(c => c.Id != entityId))
            {
                throw new ResponseException("there is no comment with such id", 400);
            }

            return await _repository.Remove(entityId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid articleId)
        {
            var comments = await _repository.GetAll();
            return comments.Where(c => c.ArticleId == articleId).ToList();
        }

        public async Task<IEnumerable<Comment>> GetEntitiesAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Comment> GetEntityAsync(Guid entityId)
        {
            var comment = await _repository.Find(entityId);
            if (comment == null)
            {
                throw new ResponseException("not found comment", 404);
            }

            return comment;
        }

        public async Task<Comment> UpdateEntityAsync(Guid entityId, CommentRequest entityRequest)
        {
            entityRequest.Id = entityId;
            var comment = _mapper.Map<CommentRequest, Comment>(entityRequest);
            if (comment.Content.Length >= 200 )
            {
                throw new ResponseException("comments bad content length", 400);
            }
            if (!await _repository.Update(comment))
            {
                throw new ResponseException("comment did not update", 500);
            }
            return comment;
        }
    }
}
