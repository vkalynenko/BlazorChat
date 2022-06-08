namespace BlazorChatApp.DAL.Domain.Entities
{
    public class UsersDeletedMessage
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string UserId { get; set; }
    }
}
