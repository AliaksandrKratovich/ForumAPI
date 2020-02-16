using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Dal.DatabaseAccess;
using Forum.Models.ArticlesManagement;
using Forum.Models.ErrorHandling;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Forum.Dal.Repositories
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
            try
            {
                await _context.Articles.InsertOneAsync(obj);
            }
            catch
            {
                throw new DatabaseException("error adding article to database");
            }
        }

        public async Task<Article> Find(Guid id)
        {
            try
            {
                return await _context.Articles.Find(a => a.Id == id).FirstOrDefaultAsync();
            }
            catch
            {
                throw new DatabaseException("finding article in database error");
            }
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            try
            {
                return await _context.Articles.Find(new BsonDocument()).SortBy(e => e.Id).ToListAsync();
            }
            catch
            {
                throw new DatabaseException("finding articles in database error");
            }
        }

        public async Task<bool> Remove(Guid id)
        {
            DeleteResult actionResult = null;
            try
            {
                var filter = Builders<Article>.Filter.Eq(a => a.Id, id);
                actionResult = await _context.Articles.DeleteOneAsync(filter);
            }
            catch
            {
                throw new DatabaseException("deleting article from database error");
            }

            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }

        public async Task<bool> Update(Article obj)
        {
            UpdateResult actionResult = null;
            try
            {
                var filter = Builders<Article>.Filter.Eq(a => a.Id, obj.Id);

                var update = Builders<Article>
                    .Update
                    .Set(a => a.Content, obj.Content)
                    .Set(a => a.Category, obj.Category)
                    .Set(a => a.CreatedDate, obj.CreatedDate);

                actionResult = await _context.Articles.UpdateOneAsync(filter, update);
            }
            catch
            {
                throw new DatabaseException("article updating error  ");
            }

            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;
        }
    }
}
