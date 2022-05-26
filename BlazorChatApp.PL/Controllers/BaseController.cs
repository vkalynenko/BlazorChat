using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [ApiController]
    [Route("api/base")]
    public class BaseController : Controller
    {
        private readonly ILocalStorageService _storageService;
        private readonly HttpClient _httpClient;
        public BaseController(ILocalStorageService storageService, HttpClient httpClient)
        {
            _storageService = storageService;
            _httpClient = httpClient;
        }

        protected BaseController()
        {
            
        }


        private const string basePath = "https://localhost:7248/api";

        //[HttpGet]
        //public async Task<IActionResult> CheckIdentityAndRedirect(string route)
        //{
        //    var token = await  GetToken();
        //    if (token != null)
        //    {
        //        var response = await _httpClient.GetAsync($"{basePath}{route}");

        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> CheckIdentity()
        {
            var token = await GetToken();
            if (token != null)
            {
                await SetAuthorizationHeader();
                return Ok();
            }

            return NotFound();
        }


        private async Task<string> GetToken()
        {
            bool hasToken = await IsTokenInStorage();
            if (hasToken)
            {
                var token = _storageService.GetItemAsync<string>("token");
                return await token;
            }

            return "There is no token in storage";
        }

        private async Task<bool> IsTokenInStorage()
        {
            if (await _storageService.GetItemAsync<string>("token")!= null)
            { 
                return true;
            }
            return false;
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await _storageService.GetItemAsync<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }
    }
}
