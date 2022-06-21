using BlazorChatApp.DAL.Domain.Entities;
using BlazorChatApp.DAL.Models;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IMessageRepository
    {
        Task DeleteMessageFromAll(int id);
        Task UpdateMessage(int id, string newMessage, string userId);
        Task<Message> FindMessage(int id);
        Task<Message> CreateMessage(int chatId, string message, string userName, string userId);
        Task<Message?> GetById(int id);
        //Task<Message> ReplyToGroup(string newMessage, string oldMessage, string userName,
        //    string currentName, string currentId, int chatId);
        Task<Message> ReplyToGroup(ReplyToGroupModel model);
        Task<Message> ReplyToUser(ReplyToUserModel model);
        Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad);
    }
}
