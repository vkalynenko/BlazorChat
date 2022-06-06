namespace BlazorChatApp.BLL.Responses
{
    public class GetCurrentUserInfo :BaseResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
