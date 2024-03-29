﻿using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IChatService
    {
        Task<bool> CreateChat(string chatName, string userid);
        Task<bool> CreatePrivateChat(string rootId, string targetId);
        Task<IEnumerable<Chat>> GetAllUserChats(string userId);
        Task<Chat> GetCurrentChat(int chatId);
        IEnumerable<Chat> GetNotJoinedChats(string userId);
        Task<bool> JoinRoom(int chatId, string userId);
        Task<Chat> GetPrivateChat(string rootId, string targetId);
        Task<int> FindPrivateChat(string senderId, string userId);
    }
}
