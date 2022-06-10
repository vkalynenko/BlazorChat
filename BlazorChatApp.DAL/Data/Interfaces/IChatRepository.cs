using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetChat(int id);
        Task CreateChat(string name, string userId);
        Task JoinRoom(int chatId, string userId);
        IEnumerable<Chat> GetNotJoinedChats(string userId);
        Task<int> CreatePrivateChat(string rootId, string targetId);
        Task<Chat> GetPrivateChat(string user1Id, string user2Id);
        Task<IEnumerable<Chat>> GetAllUserChats(string userId);
        Task<int> GetChatIdByName(string chatName);
        Task<string> CreateNewPrivateChat(string rootId, string targetId);
        Task<int> FindPrivateChat(string senderId, string userId);
    }
}
