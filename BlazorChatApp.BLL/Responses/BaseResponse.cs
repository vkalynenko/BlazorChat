using System.Net;

namespace BlazorChatApp.BLL.Responses
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
