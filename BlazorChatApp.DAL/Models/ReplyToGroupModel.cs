namespace BlazorChatApp.DAL.Models
{
    public class ReplyToGroupModel
    {
        public int ChatId { get; set; }
        public string? Reply { get; set; }
        public string? Message { get; set; }
        public string? UserName { get; set; }
        public string? SenderName { get; set; }
        public string? SenderId { get; set; }
    }
}
