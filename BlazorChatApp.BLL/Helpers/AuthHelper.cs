using System.Net.Http.Headers;

namespace BlazorChatApp.BLL.Helpers
{
    public class AuthHelper
    {
        protected async Task SetAuthorizationHeader(HttpClient httpClient)
        {
            if (TokenHolder.Token != null)
            {
                var token = TokenHolder.Token;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
