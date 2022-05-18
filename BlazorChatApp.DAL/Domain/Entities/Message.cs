using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class Message{

        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SenTime { get; set; }
        public int ChatId { get; set; }
        public string UserId { get; set; }
        public string SenderName { get; set; }

    }
}