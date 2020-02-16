using AutoMapper;
using Forum.Dal.Repositories;
using Forum.Models.CommentsManagement;
using Forum.Models.ErrorHandling;
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
            if (commentRequest.Content.Length >= 200)
            {
                throw new BadRequestException("comments bad content length");
            }
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            

            await _repository.Add(comment);
            return comment;
        }

        public async Task<bool> DeleteEntityAsync(Guid entityId)
        {
            if ((await _repository.GetAll()).All(c => c.Id != entityId))
            {
                throw new NotFoundException("there is no comment with such id");
            }

            return await _repository.Remove(entityId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid articleId)
        {
            var comments = await _repository.GetAll();
            return comments?.Where(c => c.ArticleId == articleId)?.ToList();
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
                throw new NotFoundException("not found comment");
            }

            return comment;
        }

        public async Task<Comment> UpdateEntityAsync(Guid entityId, CommentRequest entityRequest)
        {
            if (entityRequest.Content.Length >= 200)
            {
                throw new BadRequestException("comment has more than 200 symbols content length");
            }

            entityRequest.Id = entityId;
            var comment = _mapper.Map<CommentRequest, Comment>(entityRequest);
            
            if (!await _repository.Update(comment))
            {
                throw new ResponseException("comment did not update", 500);
            }
            return comment;
        }
    }
}
