using Domain.Contracts;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>
    {
        IQueryable<T> Entities { get; }
        Task<T?> GetByIdAsync(TId id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetByCondition(Expression<Func<T, bool>> incluProperties);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> FindAsync(Expression<Func<T, bool>> includeProperties);

    }
}