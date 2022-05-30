using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class RequestService : AuthHelper, IRequestService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        public RequestService(ILocalStorageService localStorage, IHttpClientFactory clientFactory)
        {
            _localStorage = localStorage;
            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateRoomAsync(string chatName)
        {
            var client = _clientFactory.CreateClient("Authorization");

            await SetAuthorizationHeader(_localStorage, client);

            var path = $"{client.BaseAddress}/chat/createRoom/{chatName}";

            var httpResponse = await client.GetAsync(path);

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
