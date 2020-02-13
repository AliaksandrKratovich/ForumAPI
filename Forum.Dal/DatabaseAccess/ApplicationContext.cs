using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Forum.Dal.DatabaseAccess
{
    public class ApplicationContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);

        }

        public IMongoCollection<Article> Articles => _database.GetCollection<Article>("Articles");

        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
    }
}
