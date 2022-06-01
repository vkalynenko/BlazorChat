using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<string> Register(RegisterDto model);
        Task<IdentityUser> Login(LoginDto model);
        IEnumerable<IdentityUser> GetUsers(string id);
    }
}
