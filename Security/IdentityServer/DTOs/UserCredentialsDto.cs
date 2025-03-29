using System.ComponentModel.DataAnnotations;

namespace IdentityService.DTOs
{
    public class UserCredentialsDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
