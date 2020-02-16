using Forum.Models.UserManagement;
using System.Threading.Tasks;

namespace Forum.Services.UserManagement
{
    public interface IUserService
   {
       Task<UserResponse> CreateUserAccount(RegisterUserRequest registerUser);

       Task<UserResponse> AuthenticateUser(LoginUserRequest loginUser);
   }
}
