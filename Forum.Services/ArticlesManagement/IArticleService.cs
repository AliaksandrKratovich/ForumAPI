using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Forum.Models.ArticlesManagement;

namespace Forum.Services.ArticlesManagement
{
    public interface IArticleService : IEntityService<Article, ArticleRequest>
    {
        Task<IEnumerable<Article>> GetArticlesByOccurrenceAsync(string title, string userName, string category);
    }
}
