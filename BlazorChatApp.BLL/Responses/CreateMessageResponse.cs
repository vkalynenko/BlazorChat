using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Responses
{
    public class CreateMessageResponse : BaseResponse
    {
        public Message? Message { get; set; }
    }
}
