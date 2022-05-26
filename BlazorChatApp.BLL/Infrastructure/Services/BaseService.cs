using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public abstract class BaseService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public BaseService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task SetAuthorizationHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }
    }
}
