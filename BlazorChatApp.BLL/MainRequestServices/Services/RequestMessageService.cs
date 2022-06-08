using System.Net;
using System.Net.Http.Json;
using System.Text;
using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.MainRequestServices.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.Domain.Entities;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace BlazorChatApp.BLL.MainRequestServices.Services
{
    public class RequestMessageService : AuthHelper, IRequestMessageService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _localStorageService;

        public RequestMessageService(IHttpClientFactory clientFactory,
            ILocalStorageService localStorageService) : base(localStorageService)
        {
            _clientFactory = clientFactory;
            _localStorageService = localStorageService;
        }
        public async Task<CreateMessageResponse> SendMessage(int chatId, string roomName, string message)
        {
            var messageDto = new MessageDto { ChatId = chatId, Message = message, RoomName = roomName };
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new CreateMessageResponse { IsAuthenticated = false, StatusCode = HttpStatusCode.Unauthorized };
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
        public async Task<GetAllMessagesFromChat> GetAllMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllMessagesFromChat { StatusCode = HttpStatusCode.Unauthorized, Messages = null };
            var path = $"{client.BaseAddress}/message/getAllMessages/{chatId}/{quantityToSkip}/{quantityToLoad}";
            var messages = await client.GetAsync(path);
            return new GetAllMessagesFromChat
            {
                StatusCode = messages.StatusCode,
                Messages = messages.Content.ReadFromJsonAsync<List<Message>>()
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
