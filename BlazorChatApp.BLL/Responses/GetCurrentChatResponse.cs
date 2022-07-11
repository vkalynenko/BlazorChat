using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Responses
{
    public class GetCurrentChatResponse :BaseResponse
    {
        public Chat? Chat { get; set; }
    }
}
