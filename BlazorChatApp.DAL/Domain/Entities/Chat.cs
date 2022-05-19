using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
            Users = new List<ChatUser>();
        }

        public int Id { get; set; }
        public string ChatName { get; set; }
        public ChatType Type { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> Users { get; set; }
        public bool IsUserInChat(string userId)
        {
            return Users.Any(user => user.UserId == userId);
        }

    }
}
