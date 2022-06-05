using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.Data.Interfaces;

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
            catch
            {
                return false;
            }
        }
    }
}
