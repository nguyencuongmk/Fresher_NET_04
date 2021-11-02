using leave_management.Data;
using System;
using System.Threading.Tasks;

namespace leave_management.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<LeaveType> LeaveTypes { get; }

        IGenericRepository<LeaveAllocation> LeaveAllocations { get; }

        IGenericRepository<LeaveRequest> LeaveRequests { get; }

        Task Save();
    }
}