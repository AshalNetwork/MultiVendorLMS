using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository
    {
        Task<T?> GetById<T>(Guid id) where T : class;
        IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : class;
        Task<List<T>> FindListAsync<T>(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default) where T : class;
        Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken) where T : class;
        Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression, string includeProperties) where T : class;
        Task<T> Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void UpdateRange<T>(IEnumerable<T> entities) where T : class;
        void Delete<T>(T entity) where T : class;
    }
}
