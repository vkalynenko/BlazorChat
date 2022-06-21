using BlazorChatApp.DAL.CustomExceptions;
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
            var chatFromDb = await _context.Chats
                .FirstOrDefaultAsync(x => x.ChatName == name);

            if (chatFromDb != null)
            {
                throw new ChatIsAlreadyExistsException("Chat is already exists!");
            }
            var chat = new Chat
            {
                ChatName = name,
                Type = ChatType.Public
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
            });

            await _context.Chats.AddAsync(chat);
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

            await _context.Chats.AddAsync(chat);
            return chat.Id;
        }

        public async Task<int> GetChatIdByName(string chatName)
        {
            var entity = await _context.Chats.FirstOrDefaultAsync(x => x.ChatName == chatName);
            return entity.Id;
        }

        public async Task<string> CreateNewPrivateChat(string rootId, string targetId)
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

            await _context.Chats.AddAsync(chat);
            return chat.ChatName;
        }

        public async Task<Chat> GetChat(int id) 
        {
            var chat = await _context.Chats
                .FirstOrDefaultAsync(x => x.Id == id);
            if (chat == null)
            {
                throw new NullReferenceException();
            }
            return chat;
        }

        public IEnumerable<Chat> GetNotJoinedChats(string userId)
        {
            return _context.Chats
                .Include(x => x.Users)
                .Where(x => x.Users.All(y => y.UserId != userId) && x.Type != ChatType.Private).ToList();
        }

        public async Task<IEnumerable<Chat>> GetAllUserChats(string userId)
        {
            return await _context.Chats
                .Where(x => x.Users
                    .Any(y => y.UserId == userId))
                .ToListAsync();
        }

        public async Task<Chat> GetPrivateChat(string user1Id, string user2Id)
        {
            var chats = await _context.Chats.Include(x => x.Users)
                .Where(chat => chat.Type.Equals(ChatType.Private))
                .ToListAsync();

            return chats.FirstOrDefault(chat => chat.IsUserInChat(user1Id) && chat.IsUserInChat(user2Id));
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
            };
            await _context.ChatUsers.AddAsync(chatUser);
        }
        public async Task<int> FindPrivateChat(string senderId, string userId)
        {
            var chats = await _context.Chats.Include(x => x.Users)
                .Where(c => c.Type.Equals(ChatType.Private))
                .ToListAsync();
            var chat = chats.FirstOrDefault(x => x.IsUserInChat(senderId) && x.IsUserInChat(userId));

            if (chat == null)
            {
                return 0;
            }
            else
            {  
                return chat.Id;
            }
          
        }
    }
}
