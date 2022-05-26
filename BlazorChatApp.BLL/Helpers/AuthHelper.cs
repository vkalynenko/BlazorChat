using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Helpers
{
    public static class AuthHelper
    {
        public static async Task SetAuthorizationHeader(ILocalStorageService localStorage, HttpClient httpClient)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
