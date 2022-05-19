using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IMessageRepository
    {
        Task DeleteMessageFromAll(int id);
        //Task DeleteOnlyFromUser(int messageId, string userId);
        Task UpdateMessage(int id, string newMessage);
        Task<Message> FindMessage(int id);
        Task<Message> CreateMessage(int chatId, string message, string userName, string userId);
    }
}
