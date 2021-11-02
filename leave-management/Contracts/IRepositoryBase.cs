using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace leave_management.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<bool> Create(T entity);

        Task<ICollection<T>> FindAll();

        Task<bool> Update(T entity);

        Task<bool> Delete(T entity);

        Task<T> FindById(int id);

        Task<bool> IsExists(int id);

        Task<bool> Save();
    }

    public interface IGenericRepository<T> where T : class
    {
        Task Create(T entity);

        Task<IList<T>> FindAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
            );

        void Update(T entity);

        void Delete(T entity);

        Task<T> Find(
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
            );

        Task<bool> IsExists(Expression<Func<T, bool>> expression = null);
    }
}