using BlazorChatApp.BLL.Contracts.DTOs;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string userName, string password);
        Task<string> RegisterAsync(string userName, string password, string confirmPassword);
        Task LogOutAsync();
    }
}
