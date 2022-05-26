using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.BLL.Contracts.DTOs
{
    public class LoginDto 
    {
        [Required(ErrorMessage = "Enter the username")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Enter the password")]
        public string? Password { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
