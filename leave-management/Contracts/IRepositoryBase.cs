using System.Collections.Generic;

namespace leave_management.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        bool Create(T entity);

        ICollection<T> FindAll();

        bool Update(T entity);

        bool Delete(T entity);

        T FindById(int id);

        bool isExists(int id);

        bool Save();
    }
}