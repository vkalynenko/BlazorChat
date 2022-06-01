using System.Net;

namespace BlazorChatApp.BLL.Responses
{
    public class CreateChatResponse : BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
