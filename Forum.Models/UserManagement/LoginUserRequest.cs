using System.ComponentModel.DataAnnotations;

namespace Forum.Models.UserManagement
{
    public class LoginUserRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
