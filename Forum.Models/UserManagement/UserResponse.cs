using System;

namespace Forum.Models.UserManagement
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

    }
}
