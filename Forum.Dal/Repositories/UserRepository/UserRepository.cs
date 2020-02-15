using System.Threading.Tasks;
using Forum.Dal.DatabaseAccess;
using Forum.Models.ArticlesManagement;
using Forum.Models.UserManagement;
using Forum.WebApi.ErrorHandling;
using MongoDB.Driver;

namespace Forum.Dal.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<User> Find(string email)
        {
            try
            {
                return await _context.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
            }
            catch
            {
                throw new ResponseException("finding user error", 500);
            }
        }

        public async Task<User> Exists(string email, string userName)
        {
            try
            {
               return await _context.Users.Find(x => x.Email == email || x.UserName == userName).FirstOrDefaultAsync();
            }
            catch
            {
                throw new ResponseException("finding user error", 500);
            }
        }
        public async Task CreateUser(User user)
        {
            try
            {
                await _context.Users.InsertOneAsync(user);
            }
            catch
            {
                throw new ResponseException("error adding user to database", 500);
            }
        }
    }
}
