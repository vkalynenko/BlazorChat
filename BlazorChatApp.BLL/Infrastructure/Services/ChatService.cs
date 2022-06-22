using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class ChatService : IChatService
    {

        private readonly IUnitOfWork _unitOfWork;
        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateChat(string chatName, string userId)
        {
            try
            {
                await _unitOfWork.Chat.CreateChat(chatName, userId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (ChatIsAlreadyExistsException)
            {
                return false;
            }
        }

        public async Task<bool> CreatePrivateChat(string rootId, string targetId)
        {
            try
            {
                await _unitOfWork.Chat.CreatePrivateChat(rootId, targetId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<IEnumerable<Chat>> GetAllUserChats(string userId)
        {
            try
            {
                IEnumerable<Chat> chats = await _unitOfWork.Chat.GetAllUserChats(userId);
                return chats;
            }
            catch(UserDoesNotExistException)
            {
                return new List<Chat>();
            }
        }
        public async Task<Chat> GetCurrentChat(int chatId)
        {
            try
            {
                Chat chat = await _unitOfWork.Chat.GetChat(chatId);
                return chat;
            }
            catch(ChatDoesNotExistException)
            {
                return new Chat();
            }
        }

        public IEnumerable<Chat> GetNotJoinedChats(string userId)
        {
            try
            {
                IEnumerable<Chat> chats = _unitOfWork.Chat.GetNotJoinedChats(userId);
                return chats;
            }
            catch (ChatsDoNotExistException)
            {
                return new List<Chat>();
            }
        }

        public async Task<bool> JoinRoom(int chatId, string userId)
        {
            try
            {
                await _unitOfWork.Chat.JoinRoom(chatId, userId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (ChatDoesNotExistException)
            {
                return false;
            }
            catch (UserDoesNotExistException)
            {
                return false;
            }
        }

        public async Task<Chat> GetPrivateChat(string rootId, string targetId)
        {
            return await _unitOfWork.Chat.GetPrivateChat(rootId, targetId);
        }

        public async Task<int> FindPrivateChat(string senderId, string userId)
        {
            var chatId = await _unitOfWork.Chat.FindPrivateChat(senderId, userId);

            if (chatId == 0)
            {
                var chatName = await _unitOfWork.Chat.CreateNewPrivateChat(senderId, userId);
                await _unitOfWork.SaveChangesAsync();
                chatId = await _unitOfWork.Chat.GetChatIdByName(chatName);
                return chatId;
            }
            return chatId;
        }
    }
}
