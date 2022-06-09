using System.Net;
using System.Net.Http.Json;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.MainRequestServices.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.Domain.Entities;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.MainRequestServices.Services
{
    public class RequestChatService : AuthHelper, IRequestChatService
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _localStorageService;

        public RequestChatService(IHttpClientFactory clientFactory,
            ILocalStorageService localStorageService) : base(localStorageService)
        {
            _clientFactory = clientFactory;
            _localStorageService = localStorageService;
        }
        public async Task<CreateChatResponse> CreateRoomAsync(string chatName)
        {
            var client = await ClientWithAuthHeader();
            var path = $"{client.BaseAddress}/chat/createRoom/{chatName}";
            if (client.DefaultRequestHeaders.Authorization == null)
                return new CreateChatResponse
                    {StatusCode = HttpStatusCode.Unauthorized };
            var httpResponse = await client.GetAsync(path);
            return new CreateChatResponse
            {
                StatusCode = httpResponse.StatusCode
            };
        }
        public async Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId)
        {
            var client = await ClientWithAuthHeader();

            if (client.DefaultRequestHeaders.Authorization == null)
                return new CreateChatResponse {StatusCode = HttpStatusCode.Unauthorized};

            var path = $"{client.BaseAddress}/chat/createPrivateRoom/{targetId}";
            var httpResponse = await client.GetAsync(path);
            return new CreateChatResponse {StatusCode = httpResponse.StatusCode};
        }

        public async Task<GetAllUserChatsResponse> GetAllUserChats()
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllUserChatsResponse {StatusCode = HttpStatusCode.Unauthorized};

            var path = $"{client.BaseAddress}/chat/getAllUserChats";
            var httpResponse = await client.GetAsync(path);

            return new GetAllUserChatsResponse
            {
                StatusCode = httpResponse.StatusCode,
                Chats = httpResponse.Content.ReadFromJsonAsync<List<Chat>>()
            };
        }

        public async Task<GetAllChatsResponse> GetAllChats()
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllChatsResponse { StatusCode = HttpStatusCode.Unauthorized};
            var path = $"{client.BaseAddress}/chat/getAllChats";
            var httpsResponse = await client.GetAsync(path);
            return new GetAllChatsResponse
            {
                StatusCode = httpsResponse.StatusCode,
                Chats = httpsResponse.Content.ReadFromJsonAsync<List<Chat>>()
            };
        }

        public async Task<BaseResponse> JoinRoom(int chatId)
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new BaseResponse {StatusCode = HttpStatusCode.Unauthorized };
            var path = $"{client.BaseAddress}/chat/joinRoom/{chatId}";
            var httpResponse = await client.GetAsync(path);
            return new BaseResponse {StatusCode = httpResponse.StatusCode,};
        }

        public async Task<GetCurrentChatResponse> GetCurrentChat(int chatId)
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetCurrentChatResponse
                {StatusCode = HttpStatusCode.Unauthorized};
            var path = $"{client.BaseAddress}/chat/getCurrentChat/{chatId}";
            var httpResponse = await client.GetAsync(path);
            return new GetCurrentChatResponse
            {
                StatusCode = httpResponse.StatusCode,
                Chat = await httpResponse.Content.ReadFromJsonAsync<Chat>()
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
