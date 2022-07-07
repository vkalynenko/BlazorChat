using System.Net;
using System.Net.Http.Json;
using System.Text;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Models;
using BlazorChatApp.BLL.RequestServices.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.CustomExtensions;
using BlazorChatApp.DAL.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BlazorChatApp.BLL.RequestServices.Services
{
    public class RequestUserService : AuthHelper, IRequestUserService
    {
        private readonly IHttpClientFactory _clientFactory;
        public RequestUserService(IHttpClientFactory clientFactory,
            ILocalStorageService localStorageService) : base(localStorageService)
        {
            _clientFactory = clientFactory;
        }
        public async Task<GetAllUsersResponse> GetOtherUsersAsync()
        {
            var client = await ClientWithAuthHeader();
            var path = $"{client.BaseAddress}/user/getOtherUsers";

            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetAllUsersResponse {StatusCode = HttpStatusCode.Unauthorized};

            var httpResponse = await client.GetAsync(path);
            return new GetAllUsersResponse
            {
                StatusCode = httpResponse.StatusCode,
                Users = httpResponse.Content.ReadFromJsonAsync<List<IdentityUser>>()
            };
        }

        public async Task<SaveProfileResponse> SaveUserProfileInfo(BrowserImageFile profile)
        {
            var client = await ClientWithAuthHeader();
            var path = $"{client.BaseAddress}/user/saveProfile";

            if (client.DefaultRequestHeaders.Authorization == null)
                return new SaveProfileResponse {StatusCode = HttpStatusCode.Unauthorized,
                    IsSavingSuccessful = false};

            //var httpResponse = await client.PostAsync($"{path}",
            //    new StringContent(JsonConvert.SerializeObject(profile),
            //        Encoding.UTF8, "application/json"));
            HelpProfileModel model = new HelpProfileModel
            {
                Type = profile.Type,
                Name = profile.Name,
                Data = profile.Data
            };

            var httpResponse = await client.PostAsync(path,new StringContent(
                    JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
                );

            if (httpResponse.IsSuccessStatusCode)
            {
                return new SaveProfileResponse {IsSavingSuccessful = true, 
                    StatusCode = httpResponse.StatusCode};
            }

            return new SaveProfileResponse {IsSavingSuccessful = false};
        }

        public async Task<GetCurrentUserInfo> GetUserInfo()
        {
            var client = await ClientWithAuthHeader();
            if (client.DefaultRequestHeaders.Authorization == null)
                return new GetCurrentUserInfo { StatusCode = HttpStatusCode.Unauthorized};
            var pathToGetUserId = $"{client.BaseAddress}/user/getUserId"; 
            var userId = await client.GetAsync(pathToGetUserId);
            var pathToGetUserName = $"{client.BaseAddress}/user/getUserName";
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
