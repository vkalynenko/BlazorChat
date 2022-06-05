using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.BLL.Contracts.DTOs
{
    public class MessageDto
    {
        public int ChatId { get; set; }
        [Required]
        public string Message { get; set; }
        public string RoomName { get; set; }
    }
}
