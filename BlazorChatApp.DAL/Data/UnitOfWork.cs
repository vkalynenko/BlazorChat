using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlazorChatAppContext _context;  
        private GenericRepository<Chat> _chatRepository;
        private GenericRepository<Message> _messageRepository;

        public UnitOfWork(
            BlazorChatAppContext appContext, 
            GenericRepository<Chat> chatRepository, 
            GenericRepository<Message> messageRepository)
        {
            _context = appContext;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
        }

        public GenericRepository<Chat> ChatRepository
        {
            get
            {

                if (_chatRepository == null)
                {
                    _chatRepository = new GenericRepository<Chat>(_context);
                }
                return _chatRepository;
            }
        }

        public GenericRepository<Message> MessageRepository
        {
            get
            {

                if (_messageRepository == null)
                {
                    _messageRepository = new GenericRepository<Message>(_context);
                }
                return _messageRepository;
            }
        }


        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        } 
    }
}
