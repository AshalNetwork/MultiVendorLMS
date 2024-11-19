using Core.Abstractions;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository(AppDbContext _context) : IRepository
    {
        
        public async Task<T?> GetById<T>(Guid id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : class
        {
            var query = _context.Set<T>().Where(expression);
            return orderBy != null ? orderBy(query) : query;
        }

        public async  Task<List<T>> FindListAsync<T>(Expression<Func<T, bool>>? expression, Func<IQueryable<T>,
            IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default)
            where T : class
        {
            var query = expression != null ? _context.Set<T>().Where(expression) : _context.Set<T>();
            return  orderBy != null
                ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);
        }

        public async Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken) where T : class
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression, string includeProperties) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            query = includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty)
                => current.Include(includeProperty));

            return query.FirstOrDefaultAsync(expression);
        }

        public async Task<T> Add<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
            return  entity;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
