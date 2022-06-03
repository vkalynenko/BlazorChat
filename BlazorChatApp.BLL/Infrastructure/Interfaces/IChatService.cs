using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IChatService
    {
        Task<bool> CreateChat(string chatName, string userid);
        Task<bool> CreatePrivateChat(string rootId, string targetId);
        IEnumerable<Chat> GetAllUserChats(string userId);
        Task<Chat> GetCurrentChat(string userId, int chatId);
        IEnumerable<Chat> GetAllChats(string userId);
        Task<bool> JoinRoom(int chatId, string userId);
    }
}
