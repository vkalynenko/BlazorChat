using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public string? ChatName { get; set; }
        public ChatType Type { get; set; }
        public ICollection<Message>? Messages { get; set; }

    }
}
