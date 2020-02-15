using AutoMapper;
using Forum.Dal.Repository;
using Forum.Models.ArticlesManagement;
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

            var article = _mapper.Map<ArticleRequest, Article>(articleRequest);
            if (article.Content.Length >= 2000)
            {
                throw new ResponseException("articles bad content length", 400);
            }

            var articles = await _repository.GetAll();
            if (articles.Select(a => a.Title).Contains(articleRequest.Title))
            {
                throw new ResponseException("article already exists with such title", 400);
            }
            if (article == null)
                throw new ResponseException("article wasn't created", 400);


            await _repository.Add(article);
            return article;
        }

        public async Task<bool> DeleteEntityAsync(Guid articleId)
        {
            if ((await _repository.GetAll()).All(c => c.Id != articleId))
            {
                throw new ResponseException("there is no article with such id", 400);
            }
            return await _repository.Remove(articleId);
        }

        public async Task<Article> GetEntityAsync(Guid articleId)
        {
            var article = await _repository.Find(articleId);
            if (article == null)
            {
                throw new ResponseException("not found article", 404);
            }

            return article;
        }

        public async Task<IEnumerable<Article>> GetEntitiesAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Article> UpdateEntityAsync(Guid articleId, ArticleRequest articleRequest)
        {
            var article = _mapper.Map<ArticleRequest, Article>(articleRequest);
            if (article.Content.Length >= 200)
            {
                throw new ResponseException("articles bad content length", 400);
            }
            
            if (!await  _repository.Update(article))
            {
                throw new ResponseException("article did not update", 500);
            }
            return article;
        }

        public async Task<IEnumerable<Article>> GetArticlesByOccurrenceAsync(string title, string userName, string category)
        {
            var articles = await _repository.GetAll();
            var filterArticles = articles?.Where(s => (title != null && s.Title.IndexOf(title) != -1) ||
                                                      (userName != null && s.UserName.IndexOf(userName) != -1)).ToList();

            if (category != null)
            {
                category = category.ToLower();
                var filterArticlesByCategories = articles?.Where(v => v.Category.ToString().ToLower().Contains(category)).ToList();

                filterArticles = filterArticles
                   ?.Concat(filterArticlesByCategories).ToList();
            }
            return filterArticles?.Count != 0 ? filterArticles : null;
        }
    }
}
