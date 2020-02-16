using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Dal.DatabaseAccess;
using Forum.Models.CommentsManagement;
using Forum.Models.ErrorHandling;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Forum.Dal.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly ApplicationContext _context;

        public CommentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Add(Comment obj)
        {
            try
            {
                await _context.Comments.InsertOneAsync(obj);
            }
            catch
            {
                throw new DatabaseException("adding comment to database error");
            }
        }

        public async Task<Comment> Find(Guid id)
        {
            try
            {
                return await _context.Comments.Find(art => art.Id == id).FirstOrDefaultAsync();
            }
            catch
            {
                throw new DatabaseException("finding comment error");
            }
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            try
            {
                return await _context.Comments.Find(new BsonDocument()).SortBy(e => e.Id).ToListAsync();
            }
            catch
            {
                throw new DatabaseException("finding comments error");
            }
        }

        public async Task<bool> Remove(Guid id)
        {
            DeleteResult actionResult = null;
            try
            {
                var filter = Builders<Comment>.Filter.Eq(a => a.Id, id);
                actionResult = await _context.Comments.DeleteOneAsync(filter);
            }
            catch
            {
                throw new DatabaseException("deleting comment error");
            }


            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }


        public async Task<bool> Update(Comment obj)
        {
            UpdateResult actionResult = null;
            try
            {
                var filter = Builders<Comment>.Filter.Eq(a => a.Id, obj.Id);

                var update = Builders<Comment>
                    .Update
                    .Set(a => a.Content, obj.Content)
                    .Set(a => a.CreatedDate, obj.CreatedDate);

                actionResult = await _context.Comments.UpdateOneAsync(filter, update);
            }
            catch
            {
                throw new DatabaseException("updating comment error");
            }

            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;
        }
    }
}
