using System.Net;

namespace BlazorChatApp.BLL.Responses
{
    public class BaseResponse
    {
        public bool IsAuthenticated { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
