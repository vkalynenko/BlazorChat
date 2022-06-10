using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.BLL.Contracts.DTOs
{
    public class MessageDto
    {
        public int ChatId { get; set; }
        [Required]
        public string? InputField { get; set; }
        public int MessageId { get; set; }
        public string? UserId { get; set; }
    }
}
