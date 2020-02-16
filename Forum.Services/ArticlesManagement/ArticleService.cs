using AutoMapper;
using Forum.Dal.Repositories;
using Forum.Models.ArticlesManagement;
using Forum.Models.ErrorHandling;
using Forum.WebApi.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services.ArticlesManagement
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _repository;
        private readonly IMapper _mapper;
        public ArticleService(IRepository<Article> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Article> CreateEntityAsync(ArticleRequest articleRequest)
        {
            if (articleRequest.Content.Length >= 2000)
            {
                throw new BadRequestException("articles bad content length");
            }
            var article = _mapper.Map<ArticleRequest, Article>(articleRequest);

            var articles = await _repository.GetAll();
            if (articles.Select(a => a.Title).Contains(articleRequest.Title))
            {
                throw new BadRequestException("article already exists with such title");
            }

            await _repository.Add(article);
            return article;
        }

        public async Task<bool> DeleteEntityAsync(Guid articleId)
        {
            if ((await _repository.GetAll()).All(c => c.Id != articleId))
            {
                throw new NotFoundException("there is no article with such id");
            }
            return await _repository.Remove(articleId);
        }

        public async Task<Article> GetEntityAsync(Guid articleId)
        {
            var article = await _repository.Find(articleId);
            if (article == null)
            {
                throw new NotFoundException("not found article");
            }

            return article;
        }

        public async Task<IEnumerable<Article>> GetEntitiesAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Article> UpdateEntityAsync(Guid articleId, ArticleRequest articleRequest)
        {
            if (articleRequest.Content.Length >= 2000)
            {
                throw new BadRequestException("article has  more than 2000 symbols content length");
            }
            var article = _mapper.Map<ArticleRequest, Article>(articleRequest);

            if (!await _repository.Update(article))
            {
                throw new ResponseException("article did not update", 500);
            }
            return article;
        }

        public async Task<IEnumerable<Article>> GetArticlesByOccurrenceAsync(string title, string userName, string category)
        {
            var articles = await _repository.GetAll();
            var filterArticles = articles?.Where(s => (title != null && s.Title.IndexOf(title) != -1) ||
                                                      (userName != null && s.UserName.IndexOf(userName) != -1)
                                                      ||(category != null && s.Category.ToString().ToLower().Contains(category.ToLower()))).ToList();

            return filterArticles?.Count != 0 ? filterArticles : null;
        }
    }
}
