namespace BlazorChatApp.BLL.Models
{
    public class ReplyToUserModel
    {
        public string Reply { get; set; }
        public string Message { get; set; }
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ChatId { get; set; }
    }
}
