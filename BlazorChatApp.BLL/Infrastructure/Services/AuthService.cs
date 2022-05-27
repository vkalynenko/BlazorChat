using System.Text;
using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        public AuthService(HttpClient httpClient, 
            ILocalStorageService localStorage, IHttpClientFactory clientFactory)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _clientFactory = clientFactory;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            LoginDto model = new LoginDto
            {
                UserName = userName, Password = password
            };
            var client = _clientFactory.CreateClient("Authorization");

            string path = $"{client.BaseAddress}/auth/login";
            var httpResponse = await _httpClient.PostAsync($"{path}", 
                new StringContent(JsonConvert.SerializeObject(model),
                    Encoding.UTF8, "application/json"));
            
            //var httpResponse = await client
            //    .PostAsync(path,
            //        new StringContent(JsonConvert.SerializeObject(model),
            //            Encoding.UTF8, "application/json"));


            if (httpResponse.IsSuccessStatusCode)
            {
               await SetTokenToLocalStorage(httpResponse);
               return "OK";
            }

            return "Not Ok";

        }

        public Task<string> RegisterAsync(string userName, string password, string confirmPassword)
        {
            return Task.FromResult("Ok");
        }

        public async Task LogOutAsync()
        {
            await _localStorage.ClearAsync();
        }

        private async Task SetTokenToLocalStorage(HttpResponseMessage httpResponse)
        {
            var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(httpResponseBody);

            await _localStorage.SetItemAsync("token", token.GeneratedToken);
        }

    }
}
