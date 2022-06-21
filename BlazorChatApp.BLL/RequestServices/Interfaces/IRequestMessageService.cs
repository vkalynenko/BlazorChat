using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.RequestServices.Interfaces
{
    public interface IRequestMessageService
    {
        Task<CreateMessageResponse> SendMessage(int chatId, string message);
        Task<GetAllMessagesFromChat> GetAllMessages(int chatId, int quantityToSkip, int quantityToLoad);
    }
}
