using Forum.Dal.DatabaseAccess;
using Forum.Models.ErrorHandling;
using Forum.Models.UserManagement;
using MongoDB.Driver;
using System.Threading.Tasks;

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
                throw new DatabaseException("finding user error");
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
                throw new DatabaseException("finding user error");
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
                throw new DatabaseException("error adding user to database");
            }
        }
    }
}
