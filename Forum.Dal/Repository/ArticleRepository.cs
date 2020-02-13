using Forum.Dal.DatabaseAccess;
using Forum.Models.ArticlesManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Dal.Repository
{
    public class ArticleRepository : IRepository<Article>
    {
        private readonly ApplicationContext _context;

        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Add(Article obj)
        {
             await _context.Articles.InsertOneAsync(obj);
        }

        public async Task<Article> Find(string id)
        {
            return await _context.Articles.Find(art => art.Id == new Guid(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _context.Articles.Find(new BsonDocument()).SortBy(e => e.Id).ToListAsync();
        }

        public async Task Remove(string id)
        {
            var filter = Builders<Article>.Filter.Eq(a => a.Id, new Guid(id));
            await _context.Articles.DeleteOneAsync(filter);
        }


        public async Task Update(Article obj)
        {
            var filter = Builders<Article>.Filter.Eq(a => a.Id, obj.Id);

            var update = Builders<Article>
                .Update
                .Set(a => a.Content, obj.Content)
                .Set(a=> a.CreatedDate, obj.CreatedDate);

            await _context.Articles.UpdateOneAsync(filter, update);
        }
    }
}
