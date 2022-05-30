namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IChatService
    {
        Task<bool> CreateChat(string chatName, string userid);
        Task<bool> CreatePrivateChat(string rootId, string targetId);
    }
}
