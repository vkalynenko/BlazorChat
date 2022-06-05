namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IMessageService
    {
        Task<bool> CreateMessage(int chatId, string message, string senderName, string userId);
    }
}
