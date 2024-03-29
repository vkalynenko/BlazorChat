﻿using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.DAL.CustomExtensions;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<string> Register(RegisterDto model);
        Task<IdentityUser> Login(LoginDto model);
        IEnumerable<IdentityUser> GetOtherUsers(string id);
        Task<bool> SaveProfile(BrowserImageFile model);
        Task<string> GetImageLink(string userId);
    }
}
