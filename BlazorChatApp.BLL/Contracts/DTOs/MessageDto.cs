using System.ComponentModel.DataAnnotations;
using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.Contracts.DTOs
{
    public class MessageDto
    {
        public int ChatId { get; set; }
        [Required]
        public string InputField { get; set; }
        public string RoomName { get; set; }
        public int MessageId { get; set; }
    }
}
