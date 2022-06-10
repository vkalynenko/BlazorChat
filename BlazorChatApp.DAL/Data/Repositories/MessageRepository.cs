using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatApp.DAL.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly BlazorChatAppContext _context;
        private readonly IChatRepository _chatRepository;
        public MessageRepository(BlazorChatAppContext context, IChatRepository chatRepository)
        {
            _context = context;
            _chatRepository = chatRepository;
        }

        public async Task<Message> CreateMessage(
            int chatId, string msgText, string userName, string userId)
        {
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

        public async Task<Message> ReplyToGroup(string newMessage, string oldMessage, string userName, 
            string currentName, string currentId, int chatId)
        {
            Message newReply = new Message
            {
                MessageText = $"Replied to {userName}:{oldMessage} - {newMessage}",
                SenderName = currentName,
                UserId = currentId,
                SentTime = DateTime.Now,
                ChatId = chatId,
                IsItReply = true
            };
           await _context.Messages.AddAsync(newReply);
           return newReply;
        }

        public async Task DeleteMessageFromAll(int id)
        {
            var entity = await FindMessage(id);
             _context.Messages.Remove(entity);
        }

        public async Task<Message> ReplyToUser(string newMessage, string oldMessage, string currentName,
            string currentId, string userName, string senderId, int chatId)
        {
            Message newReply = new Message
            {
                MessageText = $"Replied to {userName}:{oldMessage} - {newMessage}",
                SenderName = currentName,
                SentTime = DateTime.Now,
                UserId = currentId,
                ChatId = chatId,
                IsItReply = true
            };
            await _context.Messages.AddAsync(newReply);
            return newReply;
        }

        public async Task<bool> UpdateMessage(int id, string newMessage, string userId)
        {
            var entity = await FindMessage(id);

            if (userId == entity.UserId)
            {
                entity.MessageText = newMessage;
                _context.Messages.Update(entity);
                return true;
            }
            return false;
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
