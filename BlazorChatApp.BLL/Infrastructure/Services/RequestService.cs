using System.Net;
using System.Net.Http.Json;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.Domain.Entities;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class RequestService : AuthHelper, IRequestService
    {
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILocalStorageService _localStorageService;
    public RequestService(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : base(localStorageService)
    {
        _clientFactory = clientFactory;
        _localStorageService = localStorageService;
    }
        public async Task<CreateChatResponse> CreateRoomAsync(string chatName)
        {
            var client = _clientFactory.CreateClient("Authorization");
            var path = $"{client.BaseAddress}/chat/createRoom/{chatName}";

            await SetAuthorizationHeader(client);
            if (client.DefaultRequestHeaders.Authorization == null)
                return new CreateChatResponse()
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,

                };

            var httpResponse = await client.GetAsync(path);
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

            await SetAuthorizationHeader(client);
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllUsersResponse
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Users = null
                };

            var httpResponse  = await client.GetAsync(path);
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

            if (client.DefaultRequestHeaders.Authorization == null)
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

        public async Task<GetAllUserChatsResponse> GetAllUserChats()
        {
            var client = _clientFactory.CreateClient("Authorization");
            await SetAuthorizationHeader(client);
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllUserChatsResponse
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Chats = null
                };

            var path = $"{client.BaseAddress}/chat/getAllUserChats";
            var httpResponse = await client.GetAsync(path);

            return new GetAllUserChatsResponse
            {
                StatusCode = httpResponse.StatusCode,
                IsAuthenticated = httpResponse.IsSuccessStatusCode,
                Chats = httpResponse.Content.ReadFromJsonAsync<List<Chat>>(),
            };
        }

        public async Task<GetAllChatsResponse> GetAllChats()
        {
            var client = _clientFactory.CreateClient("Authorization");
            await SetAuthorizationHeader(client);
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllChatsResponse {StatusCode = HttpStatusCode.Unauthorized, Chats = null};
            var path = $"{client.BaseAddress}/chat/getAllChats";
            var httpsResponse = await client.GetAsync(path);
            return new GetAllChatsResponse
            {
                StatusCode = httpsResponse.StatusCode,
                IsAuthenticated = httpsResponse.IsSuccessStatusCode,
                Chats = httpsResponse.Content.ReadFromJsonAsync<List<Chat>>(),
            };
        }

        public async Task<BaseResponse> JoinRoom(int chatId)
        {
            var client = _clientFactory.CreateClient("Authorization");
            await SetAuthorizationHeader(client);
            if (client.DefaultRequestHeaders.Authorization == null)
                return new BaseResponse {IsAuthenticated = false, StatusCode = HttpStatusCode.Unauthorized};
            var path = $"{client.BaseAddress}/chat/joinRoom/{chatId}";
            var httpResponse = await client.GetAsync(path);
            return new BaseResponse
                {StatusCode = httpResponse.StatusCode, IsAuthenticated = httpResponse.IsSuccessStatusCode};

        }
    }

   
}
