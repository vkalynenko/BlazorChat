using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Helpers
{
    public class AuthHelper
    {
        private readonly ILocalStorageService _localStorageService;

        public AuthHelper(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected async Task SetAuthorizationHeader(HttpClient httpClient)
        {
            if (await _localStorageService.ContainKeyAsync("token"))
            {
                var token = await _localStorageService.GetItemAsync<string>("token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
