using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class LoginRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string Username { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string Password { get; set; }
    }
}
