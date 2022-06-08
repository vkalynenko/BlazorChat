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
        Task<Message> ReplyToGroup(string reply, string message, string userName, 
            string senderName, string senderId, int chatId);
        Task<Message> ReplyToUser(string reply, string message, string userName, 
            string userId, string senderName, string senderId);
        Task<int> FindPrivateChat(string senderId, string userId);
        Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad);
    }
}
