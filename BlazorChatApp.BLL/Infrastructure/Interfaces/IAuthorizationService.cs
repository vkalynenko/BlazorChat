using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorChatApp.BLL.Contracts.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IAuthorizationService
    {
        //Task<string> RegisterAsync(RegisterDto model);
        // Task<string> LoginAsync(LoginDto model);
        Task LogOutAsync();
        JwtSecurityToken GenerateJwtToken(List<Claim> authClaims);
       Task SetAuthorizationHeader();
    }
}