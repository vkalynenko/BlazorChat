using System.Net;
using System.Net.Http.Json;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.Domain.Entities;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class RequestService : AuthHelper, IRequestService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager NavManager;
        public RequestService(ILocalStorageService localStorage, 
            IHttpClientFactory clientFactory, HttpClient httpClient, NavigationManager navManager)
        {
            _localStorage = localStorage;
            _clientFactory = clientFactory;
            _httpClient = httpClient;
            NavManager = navManager;
        }

        public async Task<CreateChatResponse> CreateRoomAsync(string chatName)
        {
            var client = _clientFactory.CreateClient("Authorization");

            var path = $"{client.BaseAddress}/chat/createRoom/{chatName}";

            await SetAuthorizationHeader(_httpClient);
            if (_httpClient.DefaultRequestHeaders.Authorization == null)
                return new CreateChatResponse()
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,

                };

            var httpResponse = await _httpClient.GetAsync(path);

           return new CreateChatResponse
           {
               IsAuthenticated = httpResponse.IsSuccessStatusCode,
               StatusCode = httpResponse.StatusCode
           };

        }

        public async Task<GetAllUsersResponse> GetAllUsersAsync()
        {
            var client = _clientFactory.CreateClient("Authorization");

            var path = $"{client.BaseAddress}/chat/getAllUsers";

            await SetAuthorizationHeader(_httpClient);

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
                return new GetAllUsersResponse
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Users = null
                };

            var httpResponse  = await _httpClient.GetAsync(path);

            //IEnumerable<IdentityUser> result = await _httpClient
            //    .GetFromJsonAsync<List<IdentityUser>>(path);
            

            return new GetAllUsersResponse
            {
                IsAuthenticated = httpResponse.IsSuccessStatusCode,
                StatusCode = httpResponse.StatusCode,
                Users = httpResponse.Content.ReadFromJsonAsync<List<IdentityUser>>(),
            };
        }

        public async Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId)
        {
            var client = _clientFactory.CreateClient("Authorization");

            await SetAuthorizationHeader(client);

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
                return new CreateChatResponse
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                };

            var path = $"{client.BaseAddress}/chat/createPrivateRoom/{targetId}";

            var httpResponse = await client.GetAsync(path);

            return new CreateChatResponse
            {
                IsAuthenticated = httpResponse.IsSuccessStatusCode,
                StatusCode = httpResponse.StatusCode
            };
        }

        public async Task<GetAllChatsResponse> GetAllUserChats()
        {
            var client = _clientFactory.CreateClient("Authorization");

            await SetAuthorizationHeader(client);

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
                return new GetAllChatsResponse
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Chats = null
                };

            var path = $"{client.BaseAddress}/chat/getAllUserChats";

            var httpResponse = await client.GetAsync(path);

            return new GetAllChatsResponse
            {
                StatusCode = httpResponse.StatusCode,
                IsAuthenticated = httpResponse.IsSuccessStatusCode,
                Chats = httpResponse.Content.ReadFromJsonAsync<List<Chat>>(),
            };
        }

        public void Check()
        {
            
            NavManager.NavigateTo("/login");
            
        }
    }

   
}
