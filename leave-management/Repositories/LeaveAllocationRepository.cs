using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repositories
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            var leaveRequest = await FindAll();
            return leaveRequest.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeId == employeeId && x.Period == period).Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            var lastEntity = _db.LeaveAllocations.OrderByDescending(lt => lt.Id).FirstOrDefault();
            if (lastEntity != null)
            {
                _db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('dbo.LeaveAllocations', RESEED, {lastEntity.Id})");
                await _db.LeaveAllocations.AddAsync(entity);
            }
            else
            {
                _db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.LeaveAllocations', RESEED, 0)");
                await _db.LeaveAllocations.AddAsync(entity);
            }
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await _db.LeaveAllocations.ToListAsync();
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            return await _db.LeaveAllocations.Include(la => la.Employee).Include(la => la.LeaveType).FirstOrDefaultAsync(la => la.Id == id);
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = await _db.LeaveAllocations.Include(la => la.LeaveType).ToListAsync();
            return leaveAllocations.Where(la => la.EmployeeId == id && la.Period == period).ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationByEmployeeAndLeaveType(string id, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = await _db.LeaveAllocations.Include(la => la.LeaveType).ToListAsync();
            return leaveAllocations.FirstOrDefault(la => la.EmployeeId == id && la.Period == period && la.LeaveTypeId == leaveTypeId);
        }

        public async Task<bool> IsExists(int id)
        {
            var isLeaveAllocationExisting = await _db.LeaveTypes.AnyAsync(lt => lt.Id == id);
            return isLeaveAllocationExisting;
        }

        public async Task<bool> Save()
        {
            var change = await _db.SaveChangesAsync();
            return change > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}