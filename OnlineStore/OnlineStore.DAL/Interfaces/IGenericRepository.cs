using System.Linq.Expressions;

namespace OnlineStore.DAL.Interfaces
{
    public interface IGenericRepository
    {
        Task<T> GetAsync<T>(long id) where T : class, IEntity;
        Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;
        Task<T> GetAsync<T>(Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes) where T : class, IEntity;

        Task<IList<T>> ListAsync<T>() where T : class, IEntity;
        Task<IList<T>> ListAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;
        Task<IList<T>> ListAsync<T>(Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes) where T : class, IEntity;

        Task AddAsync<T>(T entity) where T : class, IEntity;
        Task UpdateAsync<T>(T entity) where T : class, IEntity;
        Task DeleteAsync<T>(T entity) where T : class, IEntity;
        Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;
    }
}
