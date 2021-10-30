using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace leave_management.Repositories
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeId == employeeId && x.Period == period).Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            return _db.LeaveAllocations.ToList();
        }

        public LeaveAllocation FindById(int id)
        {
            return _db.LeaveAllocations.Include(la => la.Employee).Include(la => la.LeaveType).FirstOrDefault(la => la.Id == id);
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = _db.LeaveAllocations.Include(la => la.LeaveType).ToList();
            return leaveAllocations.Where(la => la.EmployeeId == id && la.Period == period).ToList();
        }

        public bool isExists(int id)
        {
            var isLeaveAllocationExisting = _db.LeaveTypes.Any(lt => lt.Id == id);
            return isLeaveAllocationExisting;
        }

        public bool Save()
        {
            var change = _db.SaveChanges();
            return change > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}