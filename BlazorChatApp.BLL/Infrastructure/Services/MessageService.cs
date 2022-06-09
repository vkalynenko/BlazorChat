using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Models;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMessage(int chatId, string message, string senderName, string userId)
        {
            try
            {
                await _unitOfWork.Message.CreateMessage(chatId, message, senderName, userId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteMessageFromAll(int messageId)
        {
            try
            {
                await _unitOfWork.Message.DeleteMessageFromAll(messageId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Message?> GetMessage(int id)
        {
            return await _unitOfWork.Message.GetById(id);
        }

        public async Task<Message?> EditMessage(int id, string message, string userId)
        {
            try
            {
                var entity = await _unitOfWork.Message.UpdateMessage(id, message, userId);
                await _unitOfWork.SaveChangesAsync();
                return entity;

            }
            catch
            {
                return new Message();
            }
        }

        public async Task<Message> ReplyToGroup(ReplyToGroupModel model)
        {
            var message = await _unitOfWork.Message.ReplyToGroup(model.Reply, model.Message, 
                model.UserName, model.SenderName, model.SenderId, model.ChatId);
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        public async Task<Message> ReplyToUser(ReplyToUserModel model)
        {
            var message = await _unitOfWork.Message.ReplyToUser(model.Reply, model.Message, model.UserName, model.UserId,
                model.SenderName, model.SenderId);
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        public async Task<int> FindPrivateChat(string senderId, string userId)
        {
            return await _unitOfWork.Message.FindPrivateChat(senderId, userId);
        }

        public async Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            return await _unitOfWork.Message.GetMessages(chatId, quantityToSkip, quantityToLoad);
        }
    }
}
