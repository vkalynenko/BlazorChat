using System.Data;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;
using BlazorChatApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

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
            catch (ChatDoesNotExistException)
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
            catch (MessageDoesNotExistException)
            {
                return false;
            }
            catch(DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<Message> EditMessage(int id, string message, string userId)
        {
            try
            {
                 await _unitOfWork.Message.UpdateMessage(id, message, userId);
                 await _unitOfWork.SaveChangesAsync();
                 var entity = await _unitOfWork.Message.GetById(id);
                 
                 return entity;

            }
            catch(MessageDoesNotExistException)
            {
                return new Message();
            }
        }

        public async Task<Message> ReplyToGroup(ReplyToGroupModel model)
        {
            var message = await _unitOfWork.Message.ReplyToGroup(model);
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        public async Task<Message> ReplyToUser(ReplyToUserModel model)
        {
            var message = await _unitOfWork.Message.ReplyToUser(model);
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            return await _unitOfWork.Message.GetMessages(chatId, quantityToSkip, quantityToLoad);
        }
    }
}
