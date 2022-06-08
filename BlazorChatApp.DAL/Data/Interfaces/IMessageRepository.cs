using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IMessageRepository
    {
        Task DeleteMessageFromAll(int id);
        Task<Message?> UpdateMessage(int id, string newMessage);
        Task<Message> FindMessage(int id);
        Task<Message> CreateMessage(int chatId, string message, string userName, string userId);
        Task<Message?> GetById(int id);
        Task<Message> ReplyToGroup(string newMessage, string oldMessage, string userName,
            string currentName, string currentId, int chatId);
        Task<Message> ReplyToUser(string newMessage, string oldMessage, string currentName,
            string currentId, string userName, string senderId);
        Task<int> FindPrivateChat(string senderId, string userId);
        Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad);
    }
}
