//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;

//namespace BlazorChatApp.BLL.Infrastructure.Services
//{
//    public class MessageService : BaseService
//    {
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly IConfiguration _configuration;
//        private readonly HttpClient _httpClient;
//        private readonly ILocalStorageService _localStorage;
//        public MessageService(
//            SignInManager<IdentityUser> signInManager, IConfiguration configuration,
//            HttpClient httpClient, ILocalStorageService localStorage) : base(localStorage, httpClient)
//        {
//            _signInManager = signInManager;
//            _configuration = configuration;
//            _httpClient = httpClient;
//            _localStorage = localStorage;
//        }

//        public async Task<string> Test()
//        {
//            await SetAuthorizationHeader();
//            var result = 
//        }
//    }
//}
