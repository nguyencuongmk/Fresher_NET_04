using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using ServerAPI.Services;
using System;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public IActionResult Get()
        {
            var customers = this.customerService.GetAll();
            return Ok(customers);
        }

        // POST: api/customers
        [HttpPost]
        public IActionResult Create(CreateCustomerDto customerDto)
        {
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Avatar = customerDto.Avatar,
                Address = customerDto.Address,
                City = customerDto.City,
                Age = customerDto.Age,
            };

            bool isSuccess = this.customerService.Create(customer);
            if (isSuccess)
            {
                return Ok(isSuccess);
            }
            return BadRequest();         
        }

        // PUT: api/customers
        [HttpPut]
        public IActionResult Update(Customer customer)
        {
            bool isSuccess = this.customerService.Update(customer);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(customer);
        }

        // GET: api/customer/id
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var customerExisting = this.customerService.GetById(id);
            if (customerExisting != null)
            {
                return Ok(customerExisting);
            }
            return NotFound(id);
        }

        // GET: api/customers/id
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            bool isDeleted = this.customerService.Delete(id);
            if (isDeleted) return Ok(isDeleted);
            return NotFound(id);
        }
    }
}