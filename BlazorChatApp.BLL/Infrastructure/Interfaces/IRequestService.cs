using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IRequestService
    {
        Task<CreateChatResponse> CreateRoomAsync(string chatName);
        Task<GetAllUsersResponse> GetAllUsersAsync();
        Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId);
        Task<GetAllUserChatsResponse> GetAllUserChats();
        Task<GetAllChatsResponse> GetAllChats();
        Task<BaseResponse> JoinRoom(int chatId);
        Task<GetCurrentChatResponse> GetCurrentChat(int chatId);
        Task<CreateMessageResponse> SendMessage(int chatId, string roomName, string message);
        Task<GetCurrentUserInfo> GetUserInfo();
    }
}