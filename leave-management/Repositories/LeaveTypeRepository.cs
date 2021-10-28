using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace leave_management.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(LeaveType entity)
        {
            var lastEntity = _db.LeaveTypes.OrderByDescending(lt => lt.Id).FirstOrDefault();
            if (lastEntity != null)
            {
                _db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('dbo.LeaveTypes', RESEED, {lastEntity.Id})");
                _db.LeaveTypes.Add(entity);
            }
            else
            {
                _db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.LeaveTypes', RESEED, 0)");
                _db.LeaveTypes.Add(entity);
            }
            return Save();
        }

        public bool Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return Save();
        }

        public ICollection<LeaveType> FindAll()
        {
            return _db.LeaveTypes.ToList();
        }

        public LeaveType FindById(int id)
        {
            return _db.LeaveTypes.Find(id);
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool isExists(int id)
        {
            var isLeaveTypeExisting = _db.LeaveTypes.Any(lt => lt.Id == id);
            return isLeaveTypeExisting;
        }

        public bool Save()
        {
            var change = _db.SaveChanges();
            return change > 0;
        }

        public bool Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            return Save();
        }
    }
}