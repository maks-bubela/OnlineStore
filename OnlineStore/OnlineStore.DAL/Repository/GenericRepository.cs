using OnlineStore.DAL.Context;
using OnlineStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace OnlineStore.DAL.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly OnlineStoreContext _ctx;

        public GenericRepository(OnlineStoreContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException();
        }

        public async Task<T> GetAsync<T>(long id) where T : class, IEntity
        {
            return await _ctx.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            return await _ctx.Set<T>().Where(filter).SingleOrDefaultAsync();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filter, 
            params Expression<Func<T, object>>[] includes) where T : class, IEntity
        {
            var query = _ctx.Set<T>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync();
        }


        public async Task<IList<T>> ListAsync<T>() where T : class, IEntity
        {
            return await _ctx.Set<T>().ToListAsync();
        }

        public async Task<IList<T>> ListAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            return await _ctx.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<IList<T>> ListAsync<T>(Expression<Func<T, bool>> filter, 
            params Expression<Func<T, object>>[] includes) where T : class, IEntity
        {
            var query = _ctx.Set<T>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync<T>(T entity) where T : class, IEntity
        {
            _ctx.Set<T>().Add(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class, IEntity
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class, IEntity
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            return await _ctx.Set<T>().AnyAsync(filter);
        }
    }
}
