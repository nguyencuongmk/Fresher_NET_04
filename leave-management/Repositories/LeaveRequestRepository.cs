using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace leave_management.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(LeaveRequest entity)
        {
            var lastEntity = _db.LeaveRequests.OrderByDescending(lt => lt.Id).FirstOrDefault();
            if (lastEntity != null)
            {
                _db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('dbo.LeaveRequests', RESEED, {lastEntity.Id})");
                _db.LeaveRequests.Add(entity);
            }
            else
            {
                _db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.LeaveRequests', RESEED, 0)");
                _db.LeaveRequests.Add(entity);
            }
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            return _db.LeaveRequests
                .Include(lr => lr.RequestingEmployee)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .ToList();
        }

        public LeaveRequest FindById(int id)
        {
            return _db.LeaveRequests
                .Include(lr => lr.RequestingEmployee)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .FirstOrDefault(lr => lr.Id == id);
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string id)
        {
            var requests = _db.LeaveRequests.Include(lr => lr.RequestingEmployee);
            return requests.Where(lr => lr.RequestingEmployeeId == id).ToList();
        }

        public bool isExists(int id)
        {
            var isLeaveHistoryExisting = _db.LeaveTypes.Any(lt => lt.Id == id);
            return isLeaveHistoryExisting;
        }

        public bool Save()
        {
            var change = _db.SaveChanges();
            return change > 0;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }
    }
}