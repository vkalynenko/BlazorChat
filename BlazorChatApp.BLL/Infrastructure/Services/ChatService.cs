using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.Data.Interfaces;

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
            catch
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

        

    }
}
