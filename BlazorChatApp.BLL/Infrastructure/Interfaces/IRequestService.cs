using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.BLL.Responses;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IRequestService
    {
        Task<CreateChatResponse> CreateRoomAsync(string chatName);
        Task<GetAllUsersResponse> GetAllUsersAsync();

        Task<CreateChatResponse> CreatePrivateRoomAsync(string targetId);

    }
}