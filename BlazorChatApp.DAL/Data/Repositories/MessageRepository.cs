using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using BlazorChatApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatApp.DAL.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly BlazorChatAppContext _context;
        public MessageRepository(BlazorChatAppContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateMessage(
            int chatId, string msgText, string userName, string userId)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat == null)
            {
                throw new ChatDoesNotExistException("Chat doesn't exist!");
            }
            var message = new Message
            {
                ChatId = chatId,
                MessageText = msgText,
                SenderName = userName,
                UserId = userId,
                SentTime = DateTime.Now,
                IsItReply = false,
            };

            await _context.Messages.AddAsync(message);
            return message;
        }
        public async Task<Message?> GetById(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Message> ReplyToGroup(ReplyToGroupModel model)
        {
            Message newReply = new()
            {
                MessageText = $"Replied to {model.UserName}:{model.Message} - {model.Reply}",
                SenderName = model.SenderName,
                UserId = model.SenderId,
                SentTime = DateTime.Now,
                ChatId = model.ChatId,
                IsItReply = true
            };
           await _context.Messages.AddAsync(newReply);
           return newReply;
        }

        public async Task DeleteMessageFromAll(int id)
        {
            var entity = await FindMessage(id);
            if (entity == null)
            {
                throw new MessageDoesNotExistException("Message doesn't exist");
            } 
            _context.Messages.Remove(entity);
        }

        public async Task<Message> ReplyToUser(ReplyToUserModel model)
        {
            Message newReply = new()
            {
                MessageText = $"Replied to {model.SenderName}:{model.Message} - {model.Reply}",
                SenderName = model.UserName,
                SentTime = DateTime.Now,
                UserId = model.UserId,
                ChatId = model.ChatId,
                IsItReply = true
            };
            await _context.Messages.AddAsync(newReply);
            return newReply;
        }

        public async Task UpdateMessage(int id, string newMessage, string userId)
        {
            var entity = await FindMessage(id);
            if (entity == null)
            {
                throw new MessageDoesNotExistException();
            }
            if (userId == entity.UserId)
            {
                entity.MessageText = newMessage;
                _context.Messages.Update(entity);
            }
        }

        public async Task<Message> FindMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
                return message;
            return new Message();
        }

        public async Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            return await _context.Messages.OrderByDescending(x=>x.SentTime)
                .Where(chat => chat.ChatId == chatId).Skip(quantityToSkip).Take(quantityToLoad)
                .ToListAsync();
        }
    }
}
