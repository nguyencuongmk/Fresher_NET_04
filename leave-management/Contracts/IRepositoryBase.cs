using System.Collections.Generic;
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
}