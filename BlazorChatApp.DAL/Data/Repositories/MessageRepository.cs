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

            int imageId = await GetImageId(userId);

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
                IsItReply = false,
                ImageId = imageId
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
            int imageId = await GetImageId(model.SenderId);
            Message newReply = new()
            {
                MessageText = $"Replied to {model.UserName}:{model.Message} - {model.Reply}",
                SenderName = model.SenderName,
                UserId = model.SenderId,
                ChatId = model.ChatId,
                IsItReply = true,
                ImageId = imageId
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
            int imageId = await GetImageId(model.UserId);
            Message newReply = new()
            {
                MessageText = $"Replied to {model.SenderName}:{model.Message} - {model.Reply}",
                SenderName = model.UserName,
                UserId = model.UserId,
                ChatId = model.ChatId,
                IsItReply = true,
                ImageId = imageId
            };
            await _context.Messages.AddAsync(newReply);
            return newReply;
        }

        public async Task UpdateMessage(int id, string newMessage, string userId)
        {
            var entity = await FindMessage(id);
            //var userName = _userManager.FindByIdAsync(userId).Result.UserName;
            var userName = _context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result?.UserName;
            if (entity == null)
            {
                throw new MessageDoesNotExistException();
            }

            if (userId == entity.UserId)
            {
                entity.SenderName = userName;
                entity.MessageText = newMessage;
                _context.Messages.Update(entity);
            }
        }

        public async Task<Message> FindMessage(int id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            //var test = from m in _context.Messages
            //    join u in _context.Users on m.UserId equals u.Id
            //    join i in _context.Images on u.Id equals i.UserId
            //    select new {Message = m, Url = i.ImageUrl};

            var messages = await _context.Messages
                .Include(x => x.Image)
                .OrderByDescending(x => x.SentTime)
                .Where(chat => chat.ChatId == chatId)
                .Skip(quantityToSkip)
                .Take(quantityToLoad)
                .ToListAsync();

            //var res = from m in messages
            //          join i in _context.Images on m.UserId equals i.UserId
            //          select new { Message = m, Url = i.ImageUrl }.Message;
            //return res.ToList();

            return messages;

        }

        public async Task<Image> GetImage(string userId)
        {
            var image = await _context.Images.FirstOrDefaultAsync(x => x.UserId == userId);

            if (image == null)
            {
                var newImage = new Image
                {
                    ImageUrl = "https://storageaccountchatapp.blob.core.windows.net/images/avatar.png"
                };
                return newImage;
            }
            return image;
        }

        private async Task<int> GetImageId(string userId)
        {
            int imageId;
            var image = await _context.Images.FirstOrDefaultAsync(x => x.UserId == userId);
            if (image == null)
            {
                var newImage = new Image
                {
                    UserId = userId,
                    ImageUrl = "https://storageaccountchatapp.blob.core.windows.net/images/avatar.png"
                };
                _context.Images.Add(newImage);
                imageId = newImage.Id;
            }
            else
            {
                imageId = image.Id;
            }

            return imageId;
        }
    }
}
