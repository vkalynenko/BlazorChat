using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatApp.DAL.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly BlazorChatAppContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ChatRepository(BlazorChatAppContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreateChat(string name, string userId)
        {
            var chat = new Chat
            {
                ChatName = name,
                Type = ChatType.Public
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
            });

            _context.Chats.Add(chat);

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreatePrivateChat(string rootId, string targetId)
        {
            var name1 = _userManager.FindByIdAsync(targetId).Result.UserName;
            var name2 = _userManager.FindByIdAsync(rootId).Result.UserName;

            var chat = new Chat
            {

                ChatName = $"{name1} and {name2}",
                Type = ChatType.Private,

            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId,

            });

            _context.Chats.Add(chat);

            await _context.SaveChangesAsync();

            return chat.Id;
        }

        public async Task<Chat> GetChat(int id)
        {
            return await _context.Chats
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _context.Chats
                .Include(x => x.Users)
                .Where(x => x.Users.All(y => y.UserId != userId) && x.Type != ChatType.Private);
        }
        public Chat GetPrivateChat(string user1Id, string user2Id)
        {
            return  _context.Chats.Include(x => x.Users)
                .Where(chat => chat.Type.Equals(ChatType.Private))
                .FirstOrDefault(chat => chat.IsUserInChat(user1Id) && chat.IsUserInChat(user2Id));
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
            };

            _context.ChatUsers.Add(chatUser);

            await _context.SaveChangesAsync();
        }


    }
}
