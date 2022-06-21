 using BlazorChatApp.BLL.Responses;

 namespace BlazorChatApp.BLL.RequestServices.Interfaces
{
    public interface IRequestChatService
    {
        Task<CreateChatResponse> CreateRoomAsync(string chatName);
        Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId);
        Task<GetAllUserChatsResponse> GetAllUserChats();
        Task<GetAllChatsResponse> GetNotJoinedChats();
        Task<BaseResponse> JoinRoom(int chatId);
        Task<GetCurrentChatResponse> GetCurrentChat(int chatId);
    }
}
