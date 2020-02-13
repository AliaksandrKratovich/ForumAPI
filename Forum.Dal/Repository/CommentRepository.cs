using Forum.Dal.DatabaseAccess;
using Forum.Models.CommentsManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Dal.Repository
{
    class CommentRepository : IRepository<Comment>
    {
        private readonly ApplicationContext _context;

        public CommentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Add(Comment obj)
        {
            await _context.Comments.InsertOneAsync(obj);
        }

        public async Task<Comment> Find(string id)
        {
            return await _context.Comments.Find(art => art.Id == new Guid(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _context.Comments.Find(new BsonDocument()).SortBy(e => e.Id).ToListAsync();
        }

        public async Task Remove(string id)
        {
            var filter = Builders<Comment>.Filter.Eq(a => a.Id, new Guid(id));
            await _context.Comments.DeleteOneAsync(filter);
        }


        public async Task Update(Comment obj)
        {
            var filter = Builders<Comment>.Filter.Eq(a => a.Id, obj.Id);

            var update = Builders<Comment>
                .Update
                .Set(a => a.Content, obj.Content)
                .Set(a => a.CreatedDate, obj.CreatedDate);

            await _context.Comments.UpdateOneAsync(filter, update);
        }
    }
}
