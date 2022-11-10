using Application.Interfaces.Repositories;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Application.Repositories
{
    public class RepositoryAsync<T, TId> : Interfaces.Repositories.RepositoryAsync<T, TId> where T : AuditableBaseEntity<TId>
    {
        private readonly ApplicationDbContext _context;

        public RepositoryAsync(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> includeProperties)
        {
            return await _context.Set<T>().Where(includeProperties).SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetByCondition(Expression<Func<T, bool>> incluProperties)
        {
            return await _context.Set<T>().Where(incluProperties).ToListAsync();
        }

        //virtual
        public async Task<T?> GetByIdAsync(TId id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}