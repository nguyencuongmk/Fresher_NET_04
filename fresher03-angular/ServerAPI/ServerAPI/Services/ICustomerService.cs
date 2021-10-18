using ServerAPI.Models;
using System;
using System.Collections.Generic;

namespace ServerAPI.Services
{
    public interface ICustomerService
    {
        bool Create(Customer customer);

        bool Update(Customer customer);

        bool Delete(Guid id);

        Customer GetById(Guid id);

        List<Customer> GetAll();

        List<Customer> Find(Func<Customer, bool> predicate);
    }
}