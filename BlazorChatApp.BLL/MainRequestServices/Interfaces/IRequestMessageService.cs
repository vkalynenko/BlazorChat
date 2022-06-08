using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.MainRequestServices.Interfaces
{
    public interface IRequestMessageService
    {
        Task<CreateMessageResponse> SendMessage(int chatId, string roomName, string message);
        Task<GetAllMessagesFromChat> GetAllMessages(int chatId, int quantityToSkip, int quantityToLoad);
    }
}
