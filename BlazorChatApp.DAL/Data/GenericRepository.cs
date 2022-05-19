using System.Linq.Expressions;
using BlazorChatApp.DAL.Domain.EF;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatApp.DAL.Data
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal BlazorChatAppContext _context;
        internal DbSet<TEntity> _dbSet;

        public BlazorChatAppContext AppContext { get; }
        public BlazorChatAppContext Context { get; }

        public GenericRepository(BlazorChatAppContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public GenericRepository(BlazorChatAppContext context)
        {
            Context = context;
        }

        public virtual async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity?>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<TEntity?> GetById(object id) => await _dbSet.FindAsync(id);

        public virtual async Task Insert(TEntity entity)
        {
           await  _dbSet.AddAsync(entity);
        }

        public virtual async Task Delete(object id)
        {
            TEntity? entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null) Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
