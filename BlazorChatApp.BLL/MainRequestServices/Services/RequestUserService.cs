using System.Net;
using System.Net.Http.Json;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.MainRequestServices.Interfaces;
using BlazorChatApp.BLL.Responses;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.MainRequestServices.Services
{
    public class RequestUserService : AuthHelper, IRequestUserService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _localStorageService;

        public RequestUserService(IHttpClientFactory clientFactory,
            ILocalStorageService localStorageService) : base(localStorageService)
        {
            _clientFactory = clientFactory;
            _localStorageService = localStorageService;
        }
        public async Task<GetAllUsersResponse> GetAllUsersAsync()
        {
            var client = await ClientWithAuthHeader();
            var path = $"{client.BaseAddress}/chat/getAllUsers";

            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllUsersResponse
                {
                    IsAuthenticated = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Users = null
                };

            var httpResponse = await client.GetAsync(path);
            return new GetAllUsersResponse
            {
                IsAuthenticated = httpResponse.IsSuccessStatusCode,
                StatusCode = httpResponse.StatusCode,
                Users = httpResponse.Content.ReadFromJsonAsync<List<IdentityUser>>()
            };
        }
        public async Task<GetCurrentUserInfo> GetUserInfo()
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetCurrentUserInfo { StatusCode = HttpStatusCode.Unauthorized };
            var pathToGetUserId = $"{client.BaseAddress}/message/getUserId";
            var userId = await client.GetAsync(pathToGetUserId);
            var pathToGetUserName = $"{client.BaseAddress}/message/getUserName";
            var userName = await client.GetAsync(pathToGetUserName);
            return new GetCurrentUserInfo
            {
                StatusCode = userId.StatusCode,
                UserId = await userId.Content.ReadAsStringAsync(),
                UserName = await userName.Content.ReadAsStringAsync()
            };
        }
        private async Task<HttpClient> ClientWithAuthHeader()
        {
            var client = _clientFactory.CreateClient("Authorization");
            await SetAuthorizationHeader(client);
            return client;
        }
    }
}
