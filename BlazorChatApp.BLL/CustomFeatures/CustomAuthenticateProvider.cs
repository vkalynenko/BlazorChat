using System.Security.Claims;
using BlazorChatApp.BLL.Helpers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorChatApp.BLL.CustomFeatures
{
    public class CustomAuthenticateProvider : AuthenticationStateProvider
    {
       private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1500);
            string token = await _localStorageService.GetItemAsync<string>("token");

            if (string.IsNullOrEmpty(token))
            {
                var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity() { }));
                return anonymous;
            }
            var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), 
                "jwt"));
            var loginUser = new AuthenticationState(userClaimPrincipal);
            return loginUser;
        }

        public void Notify()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
    
}
