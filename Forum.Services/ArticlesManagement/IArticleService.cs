using Forum.Models.ArticlesManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Services.ArticlesManagement
{
    public interface IArticleService : IEntityService<Article, ArticleRequest>
    {
        Task<IEnumerable<Article>> GetArticlesByOccurrenceAsync(string title, string userName, string category);
    }
}
