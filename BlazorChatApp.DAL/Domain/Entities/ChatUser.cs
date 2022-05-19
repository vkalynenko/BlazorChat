namespace BlazorChatApp.DAL.Domain.Entities
{
    public class ChatUser
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
