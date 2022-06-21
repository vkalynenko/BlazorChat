using BlazorChatApp.BLL.Models;
using BlazorChatApp.DAL.Domain.Entities;
using BlazorChatApp.DAL.Models;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IMessageService
    {
        Task<bool> CreateMessage(int chatId, string message, string senderName, string userId);
        Task<bool> DeleteMessageFromAll(int messageId);
        Task<Message> EditMessage(int id, string message, string userId);
        Task<Message> ReplyToGroup(ReplyToGroupModel model);
        Task<Message> ReplyToUser(ReplyToUserModel model);
        Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad);
    }
}
