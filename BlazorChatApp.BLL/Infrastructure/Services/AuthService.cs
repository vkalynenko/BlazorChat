using System.Text;
using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        public AuthService(HttpClient httpClient, 
            ILocalStorageService localStorage, IHttpClientFactory clientFactory, AuthenticationStateProvider customAuthenticationProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _clientFactory = clientFactory;
            _customAuthenticationProvider = customAuthenticationProvider;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            LoginDto model = new LoginDto
            {
                UserName = userName, Password = password
            };
            var client = _clientFactory.CreateClient("Authorization");

            string path = $"{client.BaseAddress}/auth/login";
            var httpResponse = await client.PostAsync($"{path}", 
                new StringContent(JsonConvert.SerializeObject(model),
                    Encoding.UTF8, "application/json"));
        

            if (httpResponse.IsSuccessStatusCode)
            {
                var token = await _localStorage.GetItemAsync<string>("token");
                TokenHolder.Token = token;
                await SetTokenToLocalStorage(httpResponse);
                return "OK";
            }

            return "Not Ok";

        }

        public async Task<string> RegisterAsync(string userName, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return "Registration failed!";
            }
            RegisterDto model = new RegisterDto()
            {
                UserName = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            

            var client = _clientFactory.CreateClient("Authorization");
            string path = $"{client.BaseAddress}/auth/register";

            var response = await _httpClient.PostAsync(path,
                new StringContent(JsonConvert.SerializeObject(model), 
                    Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
               return "Registration was successful!";
            }

            return "Registration failed!";
        }
        public async Task<string> LogOutAsync()
        {
            if (await _localStorage.ContainKeyAsync("token"))
            {
                TokenHolder.Token = null;
                await _localStorage.ClearAsync();
                return "Ok";
            }

            return "Failed!";

        }

        private async Task SetTokenToLocalStorage(HttpResponseMessage httpResponse)
        {
            var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(httpResponseBody);

            await _localStorage.SetItemAsync("token", token.GeneratedToken);
        }

    }
}
