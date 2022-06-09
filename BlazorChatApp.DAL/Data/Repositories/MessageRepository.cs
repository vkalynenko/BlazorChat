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

            _context.Messages.Add(message);
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
            var entity = await _context.Messages.FindAsync(id);
            if (entity != null) _context.Messages.Remove(entity);
        }

        public async Task<Message> ReplyToUser(string newMessage, string oldMessage, string currentName, 
            string currentId, string userName, string senderId)
        {
            var chatId =  await FindPrivateChat(senderId, currentId);
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


        public async Task<Message?> UpdateMessage(int id, string newMessage, string userId)
        {
            var entity = await _context.Messages
                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity != null)
            {
                if (userId == entity.UserId)
                {
                  entity.MessageText = newMessage;
                  _context.Messages.Update(entity);
                  return entity;
                }
            }
            return new Message();
        }

        public async Task<Message> FindMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<int> FindPrivateChat(string senderId, string userId)
        {
            var chats = await _context.Chats.Include(x => x.Users)
                .Where(c => c.Type.Equals(ChatType.Private))
                .ToListAsync();
            var chat =  chats.FirstOrDefault(x => x.IsUserInChat(senderId) && x.IsUserInChat(senderId));

            if (chat == null)
            {
                var chatId = await _chatRepository.CreatePrivateChat(userId, senderId);
                return chatId;
            }

            return chat.Id;
        }

        public async Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            return await _context.Messages.OrderByDescending(x=>x.SentTime)
                .Where(chat => chat.ChatId == chatId).Skip(quantityToSkip).Take(quantityToLoad)
                .ToListAsync();
        }
    }
}
