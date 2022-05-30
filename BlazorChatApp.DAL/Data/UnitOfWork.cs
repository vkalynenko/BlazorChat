using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;

namespace BlazorChatApp.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlazorChatAppContext _context;
        public IChatRepository Chat { get; }
        public IMessageRepository Message { get; }
        public IUserRepository User { get; }

        public UnitOfWork(BlazorChatAppContext context, 
            IChatRepository chat, IMessageRepository message, 
            IUserRepository user)
        {
            _context = context;
            Chat = chat;
            Message = message;
            User = user;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }
    }
}
