using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetChat(int id);
        Task CreateChat(string name, string userId);
        Task JoinRoom(int chatId, string userId);
        IEnumerable<Chat> GetChats(string userId);
        Task<int> CreatePrivateChat(string rootId, string targetId);
        Task<Chat> GetPrivateChat(string user1Id, string user2Id);
        IEnumerable<Chat> GetAllUserChats(string userId);
    }
}
