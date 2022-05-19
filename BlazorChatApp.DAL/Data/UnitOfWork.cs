using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlazorChatAppContext _context;  
        private GenericRepository<Chat> chatRepository;
        private GenericRepository<Message> messageRepository;

        public UnitOfWork(BlazorChatAppContext appContext)
        {
            _context = appContext;
        }
        public GenericRepository<Chat> ChatRepository
        {
            get
            {

                if (this.chatRepository == null)
                {
                    this.chatRepository = new GenericRepository<Chat>(_context);
                }
                return chatRepository;
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        } 
    }
}
