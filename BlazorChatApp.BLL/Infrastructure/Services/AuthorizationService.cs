using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlazorChatApp.BLL.Infrastructure.Services
{

    public class AuthorizationService : IAuthorizationService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public AuthorizationService(
            SignInManager<IdentityUser> signInManager, IConfiguration configuration, 
            HttpClient httpClient, ILocalStorageService localStorage) 
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                    SecurityAlgorithms.HmacSha256)
            );

            return token;
        }



        public async Task SetAuthorizationHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }

    }
}