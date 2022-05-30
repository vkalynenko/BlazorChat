using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class RequestService : AuthHelper, IRequestService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public RequestService(ILocalStorageService localStorage, IHttpClientFactory clientFactory, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _clientFactory = clientFactory;
            _httpClient = httpClient;
        }

        public async Task<bool> CreateRoomAsync(string chatName)
        {
            var client = _clientFactory.CreateClient("Authorization");

            var path = $"{client.BaseAddress}/chat/createRoom/{chatName}";

            await SetAuthorizationHeader(_localStorage, _httpClient);
            if (_httpClient.DefaultRequestHeaders.Authorization == null) return false;
      
            var httpResponse = await _httpClient.GetAsync(path);
          
            if (httpResponse.IsSuccessStatusCode) return true;

            return false;

        }

        public async Task<bool> CreatePrivateRoomAsync(string targetId)
        {
            var client = _clientFactory.CreateClient("Authorization");

            await SetAuthorizationHeader(_localStorage, client);

            var path = $"{client.BaseAddress}/chat/createPrivateRoom/{targetId}";

            var httpResponse = await client.GetAsync(path);

            if (httpResponse.IsSuccessStatusCode) return true;

            return false;

        }
    }
}
