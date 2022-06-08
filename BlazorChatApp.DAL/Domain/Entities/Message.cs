using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class Message 
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
        public int ChatId { get; set; }
        public string UserId { get; set; }
        public string SenderName { get; set; }
        public bool IsItReply { get; set; }
        public Message()
        {
            SentTime = DateTime.Now;
        }

        public IEnumerable<Message> Messages;
    }
}