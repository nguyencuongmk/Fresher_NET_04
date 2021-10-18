using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private static List<Customer> _customer;
        public CustomerService()
        {
            _customer = new List<Customer>()
            {
                new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Đỗ Trung",
                    LastName="Anh",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Hai Bà Trưng, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
                 new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Nguyễn Tuấn",
                    LastName="Anh",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Đống Đa, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
                  new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Lê Thị",
                    LastName="Hảo",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Hà Đông, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
                   new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Phạm Văn",
                    LastName="Hưng",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Hai Bà Trưng, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
                    new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Ngô Xuân",
                    LastName="Lộc",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Hai Bà Trưng, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
                     new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Đào Thị",
                    LastName="Trang",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Hai Bà Trưng, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
                      new Customer()
                {
                    Id=Guid.NewGuid(),
                    FirstName="Lê Thành",
                    LastName="Trung",
                    Avatar="https://anhdep123.com/wp-content/uploads/2021/02/anh-avatar-hai-huoc.jpg",
                    Address="Hai Bà Trưng, Hà Nội",
                    City= "Hà Nội",
                    Age=18
                },
            };
        }
        public bool Create(Customer customer)
        {
            _customer.Add(customer);
            return true;
        }

        public bool Delete(Guid id)
        {
            var customerExisting = _customer.FirstOrDefault(x => x.Id == id);
            if (customerExisting != null)
            {
                _customer.Remove(customerExisting);
                return true;
            }
            return false;
        }

        public List<Customer> Find(Func<Customer, bool> predicate)
        {
            return _customer.Where(predicate).ToList();
        }

        public List<Customer> GetAll()
        {
            return _customer;
        }

        public Customer GetById(Guid id)
        {
            return _customer.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Customer customer)
        {
            var customerExisting = _customer.FirstOrDefault(x => x.Id == customer.Id);
            if(customerExisting != null)
            {
                customerExisting.Avatar = customer.Avatar;
                customerExisting.Address = customer.Address;
                customerExisting.Age = customer.Age;
                customerExisting.City = customer.City;
                customerExisting.FirstName = customer.FirstName;
                customerExisting.LastName = customer.LastName;
                return true;
            }
            return false;
        }
    }
}