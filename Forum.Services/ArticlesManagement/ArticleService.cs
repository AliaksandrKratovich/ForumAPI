using System;
using AutoMapper;
using Forum.Dal.Repository;
using Forum.Models.ArticlesManagement;
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
                return null;
            }

            var articles = await _repository.GetAll();
            if (articles.Select(a => a.Title).Contains(articleRequest.Title))
            {
                return null;
            }

            await _repository.Add(article);
            return article;
        }

        public async Task DeleteEntityAsync(string articleId)
        {
            await _repository.Remove(articleId);
        }

        public async Task<Article> GetEntityAsync(string articleId)
        {
            return await _repository.Find(articleId);
        }

        public async Task<IEnumerable<Article>> GetEntitiesAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Article> UpdateEntityAsync(string articleId, ArticleRequest articleRequest)
        {
            var article = _mapper.Map<ArticleRequest, Article>(articleRequest);

            await _repository.Update(article);
            return article;
        }

        public async Task<IEnumerable<Article>> GetArticlesByOccurrenceAsync(string title, string userName, string category)
        {
            var articles = await _repository.GetAll();
            var filterArticlesByTitle_and_User = articles?.Where(s => (title != null && s.Title.IndexOf(title) != -1) ||
                                                      (userName != null && s.UserName.IndexOf(userName) != -1)).ToList();

            category = category?.ToLower();

            var filterArticlesByCategories= articles?.Where(v =>v.Category.ToString().ToLower().Contains(category)).ToList();
            
            var filterArticles = filterArticlesByTitle_and_User 
                .Concat(filterArticlesByCategories).ToList();

            return filterArticles.Count != 0 ? filterArticles : null;
        }
    }
}
