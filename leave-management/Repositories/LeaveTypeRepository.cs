using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LeaveType entity)
        {
            var lastEntity = _db.LeaveTypes.OrderByDescending(lt => lt.Id).FirstOrDefault();
            if (lastEntity != null)
            {
                _db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('dbo.LeaveTypes', RESEED, {lastEntity.Id})");
                await _db.LeaveTypes.AddAsync(entity);
            }
            else
            {
                _db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.LeaveTypes', RESEED, 0)");
                await _db.LeaveTypes.AddAsync(entity);
            }
            return await Save();
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            return await _db.LeaveTypes.ToListAsync();
        }

        public async Task<LeaveType> FindById(int id)
        {
            return await _db.LeaveTypes.FindAsync(id);
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsExists(int id)
        {
            var isLeaveTypeExisting = await _db.LeaveTypes.AnyAsync(lt => lt.Id == id);
            return isLeaveTypeExisting;
        }

        public async Task<bool> Save()
        {
            var change = await _db.SaveChangesAsync();
            return change > 0;
        }

        public async Task<bool> Update(LeaveType entity)
        {
             _db.LeaveTypes.Update(entity);
            return await Save();
        }
    }
}