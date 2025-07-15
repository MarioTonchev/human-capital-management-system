using System.Linq.Expressions;

namespace HCMS.Infrastructure.Repositories
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;

        IQueryable<T> AllAsReadOnly<T>() where T : class;

        Task<T?> GetByIdAsync<T>(object id) where T : class;

        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task AddAsync<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<int> SaveChangesAsync();
    }
}
