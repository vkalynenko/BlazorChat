using System.Net;
using System.Net.Http.Json;
using System.Text;
using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.Domain.Entities;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BlazorChatApp.BLL.Infrastructure.Services;

public class RequestService : AuthHelper, IRequestService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILocalStorageService _localStorageService;

    public RequestService(IHttpClientFactory clientFactory,
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
                {IsAuthenticated = false, StatusCode = HttpStatusCode.Unauthorized};
        var httpResponse = await client.GetAsync(path);
        return new CreateChatResponse
        {
            IsAuthenticated = httpResponse.IsSuccessStatusCode,
            StatusCode = httpResponse.StatusCode
        };
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

    public async Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId)
    {
        var client = await ClientWithAuthHeader();

        if (client.DefaultRequestHeaders.Authorization == null)
            return new CreateChatResponse
            {
                IsAuthenticated = false,
                StatusCode = HttpStatusCode.Unauthorized
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
        var client = await ClientWithAuthHeader();
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
            Chats = httpResponse.Content.ReadFromJsonAsync<List<Chat>>()
        };
    }

    public async Task<GetAllChatsResponse> GetAllChats()
    {
        var client = await ClientWithAuthHeader();
        if (client.DefaultRequestHeaders.Authorization == null)
            return new GetAllChatsResponse {StatusCode = HttpStatusCode.Unauthorized, Chats = null};
        var path = $"{client.BaseAddress}/chat/getAllChats";
        var httpsResponse = await client.GetAsync(path);
        return new GetAllChatsResponse
        {
            StatusCode = httpsResponse.StatusCode,
            IsAuthenticated = httpsResponse.IsSuccessStatusCode,
            Chats = httpsResponse.Content.ReadFromJsonAsync<List<Chat>>()
        };
    }

    public async Task<BaseResponse> JoinRoom(int chatId)
    {
        var client = await ClientWithAuthHeader();
        if (client.DefaultRequestHeaders.Authorization == null)
            return new BaseResponse {IsAuthenticated = false, StatusCode = HttpStatusCode.Unauthorized};
        var path = $"{client.BaseAddress}/chat/joinRoom/{chatId}";
        var httpResponse = await client.GetAsync(path);
        return new BaseResponse
        {
            StatusCode = httpResponse.StatusCode, IsAuthenticated = httpResponse.IsSuccessStatusCode
        };
    }

    public async Task<GetCurrentChatResponse> GetCurrentChat(int chatId)
    {
        var client = await ClientWithAuthHeader();
        if (client.DefaultRequestHeaders.Authorization == null)
            return new GetCurrentChatResponse
                {IsAuthenticated = false, StatusCode = HttpStatusCode.Unauthorized, Chat = null};
        var path = $"{client.BaseAddress}/chat/getCurrentChat/{chatId}";
        var httpResponse = await client.GetAsync(path);
        return new GetCurrentChatResponse
        {
            StatusCode = httpResponse.StatusCode, Chat = await httpResponse.Content.ReadFromJsonAsync<Chat>()
        };
    }

    public async Task<CreateMessageResponse> SendMessage(int chatId, string roomName, string message)
    {
        var messageDto = new MessageDto {ChatId = chatId, Message = message, RoomName = roomName};
        var client = await ClientWithAuthHeader();
        if (client.DefaultRequestHeaders.Authorization == null)
            return new CreateMessageResponse {IsAuthenticated = false, StatusCode = HttpStatusCode.Unauthorized};
        var path = $"{client.BaseAddress}/message/sendMessage";
        var httpResponse = await client.PostAsync(path,
            new StringContent(JsonConvert.SerializeObject(messageDto), Encoding.UTF8, "application/json"));
        var content = await httpResponse.Content.ReadAsStringAsync();
        return new CreateMessageResponse
        {
            StatusCode = httpResponse.StatusCode,
            Message = JsonConvert.DeserializeObject<Message>(content)
        };
    }

    public async Task<GetCurrentUserInfo> GetUserInfo()
    {
        var client = await ClientWithAuthHeader();
        if (client.DefaultRequestHeaders.Authorization == null)
            return new GetCurrentUserInfo {StatusCode = HttpStatusCode.Unauthorized};
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

    public async Task<GetAllMessagesFromChat> GetAllMessages(int chatId, int quantityToSkip, int quantityToLoad)
    {
        var client = await ClientWithAuthHeader();
        if (client.DefaultRequestHeaders.Authorization == null)
            return new GetAllMessagesFromChat {StatusCode = HttpStatusCode.Unauthorized, Messages = null};
        var path = $"{client.BaseAddress}/message/getAllMessages/{chatId}/{quantityToSkip}/{quantityToLoad}";
        var messages = await client.GetAsync(path);
        return new GetAllMessagesFromChat
        {
            StatusCode = messages.StatusCode, Messages = messages.Content.ReadFromJsonAsync<List<Message>>()
        };
    }

    private async Task<HttpClient> ClientWithAuthHeader()
    {
        var client = _clientFactory.CreateClient("Authorization");
        await SetAuthorizationHeader(client);
        return client;
    }
}