using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public LeaveAllocationController(
            ILeaveTypeRepository leaveTypeRepository, 
            ILeaveAllocationRepository leaveAllocationRepository, 
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: LeaveAllocationController
        public ActionResult Index()
        {
            var leaveTypes = _leaveTypeRepository.FindAll();
            var leaveTypesVM = _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
            var createLeaveAllocationVM = new CreateLeaveAllocationVM
            {
                NumberUpdated = 0,
                LeaveTypes = leaveTypesVM
            };
            return View(createLeaveAllocationVM);
        }

        public ActionResult SetLeave(int id)
        {
            var leaveType = _leaveTypeRepository.FindById(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var employee in employees)
            {
                if (_leaveAllocationRepository.CheckAllocation(id, employee.Id))
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
                _leaveAllocationRepository.Create(leaveAllocation);
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
        public ActionResult Details(string id)
        {
            var employee = _userManager.FindByIdAsync(id).Result;
            var employeeVM = _mapper.Map<EmployeeVM>(employee);
            var leaveAllocations = _leaveAllocationRepository.GetLeaveAllocationByEmployee(id);
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
        public ActionResult Edit(int id)
        {
            var leaveAllocation = _leaveAllocationRepository.FindById(id);
            var editLeaveAllocationVM = _mapper.Map<EditLeaveAllocationVM>(leaveAllocation);
            return View(editLeaveAllocationVM);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationVM editLeaveAllocationVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(editLeaveAllocationVM);
                }

                var leaveAllocation = _leaveAllocationRepository.FindById(editLeaveAllocationVM.Id);
                leaveAllocation.NumberOfDays = editLeaveAllocationVM.NumberOfDays;

                var isSuccess = _leaveAllocationRepository.Update(leaveAllocation);
                if (!isSuccess)
                {
                    ModelState.AddModelError("","Something went wrong");
                    return View(editLeaveAllocationVM);
                }
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
