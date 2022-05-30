using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Helpers
{
    public class AuthHelper
    {
 
      public async Task SetAuthorizationHeader(ILocalStorageService localStorage, HttpClient httpClient)
        {
            if (await localStorage.ContainKeyAsync("token"))
            {
                var token = await localStorage.GetItemAsync<string>("token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
        }

    }
}
