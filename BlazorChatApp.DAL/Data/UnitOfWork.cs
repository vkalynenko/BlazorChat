using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;

namespace BlazorChatApp.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlazorChatAppContext _context;

        public UnitOfWork(BlazorChatAppContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        } 
    }
}
