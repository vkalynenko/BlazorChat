using System.Text;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        public ChatService(HttpClient httpClient,
            ILocalStorageService localStorage, IHttpClientFactory clientFactory)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _clientFactory = clientFactory;
        }
        public async Task<bool> CreateRoomAsync(string chatName)
        {
            var client = _clientFactory.CreateClient("Authorization");

            await AuthHelper.SetAuthorizationHeader(_localStorage, _httpClient);

            string path = $"{client.BaseAddress}/chat/createRoom";

            var httpResponse = await _httpClient.PostAsync($"{path}",
                new StringContent(JsonConvert.SerializeObject(chatName),
                    Encoding.UTF8, "application/json"));


            if (httpResponse.IsSuccessStatusCode)
            {
              return true;
            }

            return false;
        }
    }
}
