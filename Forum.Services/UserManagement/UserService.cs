using AutoMapper;
using Forum.Dal.Repositories.UserRepository;
using Forum.Models.Security;
using Forum.Models.UserManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Services.UserManagement
{
    public class UserService : IUserService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IOptions<JwtOptions> jwtOptions, IUserRepository repository, IMapper mapper)
        {
            _jwtOptions = jwtOptions.Value;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> AuthenticateUser(LoginUserRequest loginUser)
        {
            var user = await _repository.Find(loginUser.Email);

            if (Authenticate(user, loginUser.Password) == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var response = _mapper.Map<User, UserResponse>(user);
            response.Token = tokenString;
            return response;
        }

        private User Authenticate(User user, string password)
        {
            return (user?.Password == password) ? user :null;
        }

        public async Task<UserResponse> CreateUserAccount(RegisterUserRequest registerUser)
        {
            if (await _repository.Exists(registerUser.Email, registerUser.UserName) != null)
            {
                return null;
            }
            var user = _mapper.Map<RegisterUserRequest, User>(registerUser);

            await _repository.CreateUser(user);
            return _mapper.Map<User, UserResponse>(user);
        }

    }
}
