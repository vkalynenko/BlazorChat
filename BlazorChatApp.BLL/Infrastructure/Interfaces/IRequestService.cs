using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.BLL.Responses;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IRequestService
    {
        Task<bool> CreateRoomAsync(string chatName);
        Task<IEnumerable<IdentityUser>> GetAllUsersAsync();

        Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId);

    }
}