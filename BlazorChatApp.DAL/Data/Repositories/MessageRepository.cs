using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
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
            var message = new Message
            {
                ChatId = chatId,
                MessageText = msgText,
                SenderName = userName,
                UserId = userId,
                SentTime = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<Message?> GetById(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Message> ReplyToGroup(string reply, string message, string userName, 
            string senderName, string senderId, int chatId)
        {
            Message newReply = new Message
            {
                MessageText = $"Replied to {userName}: {message} \n" +
                              $"{reply}",
                SenderName = senderName,
                UserId = senderId,
                SentTime = DateTime.Now,
                ChatId = chatId,

            };
           await _context.Messages.AddAsync(newReply);
           await _context.SaveChangesAsync();
           return newReply;
        }

        public async Task DeleteMessageFromAll(int id)
        {
            var entity = await _context.Messages.FindAsync(id);
            if (entity != null) _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateMessage(int id, string newMessage)
        {
            var entity = await _context.Messages
                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity != null)
            {
                entity.MessageText = newMessage;
                _context.Messages.Update(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Message> FindMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            return message;
        }
    }
}
