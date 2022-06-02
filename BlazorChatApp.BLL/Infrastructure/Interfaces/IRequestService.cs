using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IRequestService
    {
        Task<CreateChatResponse> CreateRoomAsync(string chatName);
        Task<GetAllUsersResponse> GetAllUsersAsync();
        Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId);
        Task<GetAllChatsResponse> GetAllUserChats();
        Task<GetAllChatsResponse> GetAllChatsAsync();
    }
}