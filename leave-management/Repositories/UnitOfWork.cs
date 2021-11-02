using leave_management.Contracts;
using leave_management.Data;
using System;
using System.Threading.Tasks;

namespace leave_management.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<LeaveType> _leaveTypes;
        private IGenericRepository<LeaveAllocation> _leaveAllocations;
        private IGenericRepository<LeaveRequest> _leaveRequests;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<LeaveType> LeaveTypes
        {
            get
            {
                if (_leaveTypes == null)
                {
                    _leaveTypes = new GenericRepository<LeaveType>(_context);
                }
                return _leaveTypes;
            }
        }

        public IGenericRepository<LeaveAllocation> LeaveAllocations
        {
            get
            {
                if (_leaveAllocations == null)
                {
                    _leaveAllocations = new GenericRepository<LeaveAllocation>(_context);
                }
                return _leaveAllocations;
            }
        }
       

        public IGenericRepository<LeaveRequest> LeaveRequests
        {
            get
            {
                if (_leaveRequests == null)
                {
                    _leaveRequests = new GenericRepository<LeaveRequest>(_context);
                }
                return _leaveRequests;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}