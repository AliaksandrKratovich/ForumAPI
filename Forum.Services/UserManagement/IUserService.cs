using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Forum.Models.UserManagement;

namespace Forum.Services.UserManagement
{
   public interface IUserService
   {
       Task<UserResponse> CreateUserAccount(RegisterUserRequest registerUser);

       Task<UserResponse> AuthenticateUser(LoginUserRequest loginUser);
   }
}
