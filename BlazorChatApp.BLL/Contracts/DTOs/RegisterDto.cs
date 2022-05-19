using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.BLL.Contracts.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Enter the name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter the password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter your password again")]
        public string ConfirmPassword { get; set; } 
        
    }
}
