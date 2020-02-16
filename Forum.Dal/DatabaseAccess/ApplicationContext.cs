using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Forum.Models.UserManagement;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Forum.Dal.DatabaseAccess
{
    public class ApplicationContext
    {
        private readonly IMongoDatabase _database;

        private readonly string _articleCollection;

        private readonly string _commentsCollection;

        private readonly string _usersCollection;

        public ApplicationContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);

            _articleCollection = settings.Value.ArticleCollection;
            _commentsCollection = settings.Value.CommentsCollection;
            _usersCollection = settings.Value.UserCollection;
        }

        public IMongoCollection<Article> Articles => _database.GetCollection<Article>(_articleCollection);

        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>(_commentsCollection);

        public IMongoCollection<User> Users => _database.GetCollection<User>(_usersCollection);
    }
}
