using System.Threading.Tasks;
using Forum.Models.UserManagement;

namespace Forum.Dal.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> Find(string email);

        Task<User> Exists(string email, string userName);

        Task CreateUser(User user);

    }
}
