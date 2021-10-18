using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string  FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    }
}
