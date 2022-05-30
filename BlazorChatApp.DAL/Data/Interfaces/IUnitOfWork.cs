namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IChatRepository Chat { get; }
        IMessageRepository Message { get; }
        IUserRepository User { get; }
        Task<int> SaveChangesAsync();
        
    }
}
