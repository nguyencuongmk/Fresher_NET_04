using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public LeaveAllocationController(
            ILeaveTypeRepository leaveTypeRepository, 
            ILeaveAllocationRepository leaveAllocationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: LeaveAllocationController
        public async Task<ActionResult> Index()
        {
            //var leaveTypes = await _leaveTypeRepository.FindAll();
            var leaveTypes = await _unitOfWork.LeaveTypes.FindAll();
            var leaveTypesVM = _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
            var createLeaveAllocationVM = new CreateLeaveAllocationVM
            {
                NumberUpdated = 0,
                LeaveTypes = leaveTypesVM
            };
            return View(createLeaveAllocationVM);
        }

        public async Task<ActionResult> SetLeave(int id)
        {
            //var leaveType = await _leaveTypeRepository.FindById(id);
            var leaveType = await _unitOfWork.LeaveTypes.Find(lt => lt.Id == id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var period = DateTime.Now.Year;
            foreach (var employee in employees)
            {
                //if (await _leaveAllocationRepository.CheckAllocation(id, employee.Id))
                if (await _unitOfWork.LeaveAllocations.IsExists
                    (
                        la => la.EmployeeId == employee.Id
                           && la.LeaveTypeId == leaveType.Id
                           && la.Period == period
                    ))
                    continue;

                var leaveAllocationVM = new LeaveAllocationVM
                {
                    NumberOfDays = leaveType.DefaultDay,
                    DateCreated = DateTime.Now,
                    LeaveTypeId = id,
                    EmployeeId = employee.Id,
                    Period = DateTime.Now.Year
                };

                var leaveAllocation = _mapper.Map<LeaveAllocation>(leaveAllocationVM);
                //await _leaveAllocationRepository.Create(leaveAllocation);
                await _unitOfWork.LeaveAllocations.Create(leaveAllocation);
                await _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var employeesVM = _mapper.Map<List<EmployeeVM>>(employees);
            return View(employeesVM);
        }

        // GET: LeaveAllocationController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var employee = _userManager.FindByIdAsync(id).Result;
            var employeeVM = _mapper.Map<EmployeeVM>(employee);
            var period = DateTime.Now.Year;
            //var leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationByEmployee(id);
            var leaveAllocations = await _unitOfWork.LeaveAllocations.FindAll
                (
                    expression: la => la.EmployeeId == id && la.Period == period,
                    include: q => q.Include(lr => lr.LeaveType)
                    //new List<string> { "LeaveType" }
                );
            var leaveAllocationsVM = _mapper.Map<List<LeaveAllocationVM>>(leaveAllocations);

            var leaveAllocationVM = new ViewAllocationVM
            {
                Employee = employeeVM,
                LeaveAllocations = leaveAllocationsVM
            };
            return View(leaveAllocationVM);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            //var leaveAllocation = await _leaveAllocationRepository.FindById(id);
            var leaveAllocation = await _unitOfWork.LeaveAllocations.Find
                (
                    expression: la => la.Id == id,
                    include: q => q.Include(lr => lr.Employee).Include(lr => lr.LeaveType)
                    //new List<string> { "Employee", "LeaveType" }
                );
            var editLeaveAllocationVM = _mapper.Map<EditLeaveAllocationVM>(leaveAllocation);
            return View(editLeaveAllocationVM);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditLeaveAllocationVM editLeaveAllocationVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(editLeaveAllocationVM);
                }

                //var leaveAllocation = await _leaveAllocationRepository.FindById(editLeaveAllocationVM.Id);
                var leaveAllocation = await _unitOfWork.LeaveAllocations.Find(la => la.Id == editLeaveAllocationVM.Id);
                leaveAllocation.NumberOfDays = editLeaveAllocationVM.NumberOfDays;

                //var isSuccess =  await _leaveAllocationRepository.Update(leaveAllocation);
                //if (!isSuccess)
                //{
                //    ModelState.AddModelError("","Something went wrong");
                //    return View(editLeaveAllocationVM);
                //}

                _unitOfWork.LeaveAllocations.Update(leaveAllocation);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Details), new { id = editLeaveAllocationVM.Employee.Id});
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(editLeaveAllocationVM);
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
