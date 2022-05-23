using BlazorChatApp.BLL.Contracts.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IAuthorizationService
    {
        Task<string> Register(string userName, string password);
        Task<string> Login(string userName, string password);
        Task LogOut();
    }
}
