﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace leave_management.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<LeaveHistory> LeaveHistories { get; set; }

        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
    }
}