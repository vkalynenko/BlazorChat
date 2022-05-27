using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorChatApp.BLL.Helpers;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class TestService : AuthHelper, ITestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;

        public TestService(HttpClient httpClient, ILocalStorageService localStorage, IHttpClientFactory clientFactory)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _clientFactory = clientFactory;
        }

        public string GetMessage()
        {
            var client = _clientFactory.CreateClient("Authorization");

            var path =$"{client.BaseAddress}/test/text"; 
            SetAuthorizationHeader(_localStorage, _httpClient);

            
            return "not good!";
        }
    }
}
