using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Responses
{
    public class GetAllMessagesFromChat :BaseResponse
    {
        public Task<List<dynamic>?>? Messages { get; set; }
    }
}
