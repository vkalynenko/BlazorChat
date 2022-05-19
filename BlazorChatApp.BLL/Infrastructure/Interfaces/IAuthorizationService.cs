using BlazorChatApp.BLL.Contracts.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IAuthorizationService
    {
        Task<string> Register(RegisterDto model);
        Task<string> Login(LoginDto model);
        Task LogOut();
    }
}
