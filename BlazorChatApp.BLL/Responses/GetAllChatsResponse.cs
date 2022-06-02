using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Responses
{
    public class GetAllChatsResponse : BaseResponse
    { 
        public Task<List<Chat>?>? Chats { get; set; }
    }
}
