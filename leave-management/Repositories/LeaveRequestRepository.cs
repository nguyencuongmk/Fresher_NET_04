using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LeaveRequest entity)
        {
            var lastEntity = _db.LeaveRequests.OrderByDescending(lt => lt.Id).FirstOrDefault();
            if (lastEntity != null)
            {
                _db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('dbo.LeaveRequests', RESEED, {lastEntity.Id})");
                await _db.LeaveRequests.AddAsync(entity);
            }
            else
            {
                _db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.LeaveRequests', RESEED, 0)");
                await _db.LeaveRequests.AddAsync(entity);
            }
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            return await _db.LeaveRequests
                .Include(lr => lr.RequestingEmployee)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .ToListAsync();
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            return await _db.LeaveRequests
                .Include(lr => lr.RequestingEmployee)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public async Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string id)
        {
            var requests = _db.LeaveRequests.Include(lr => lr.RequestingEmployee);
            return await requests.Where(lr => lr.RequestingEmployeeId == id).ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            var isLeaveHistoryExisting = await _db.LeaveTypes.AnyAsync(lt => lt.Id == id);
            return isLeaveHistoryExisting;
        }

        public async Task<bool> Save()
        {
            var change = await _db.SaveChangesAsync();
            return change > 0;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return await Save();
        }
    }
}