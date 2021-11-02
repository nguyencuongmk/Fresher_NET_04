using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public LeaveRequestController(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager
            )
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles ="Administrator")]
        // GET: LeaveRequestController
        public async Task<ActionResult> Index()
        {
            var leaveRequests = await _leaveRequestRepository.FindAll();
            var leaveRequestsVM = _mapper.Map<List<LeaveRequestVM>>(leaveRequests);

            var adminLeaveRequestViewVM = new AdminLeaveRequestVM
            {
                TotalRequests = leaveRequestsVM.Count,
                ApprovedRequests = leaveRequestsVM.Count(r => r.Approved == true),
                PendingRequests = leaveRequestsVM.Count(r => r.Approved == null),
                RejectRequests = leaveRequests.Count(r => r.Approved == false),
                LeaveRequests = leaveRequestsVM
            };
            return View(adminLeaveRequestViewVM);
        }

        // GET: LeaveRequestController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var leaveRequest = await _leaveRequestRepository.FindById(id);
            var leaveRequestVM = _mapper.Map<LeaveRequestVM>(leaveRequest);
            return View(leaveRequestVM);
        }

        public async Task<ActionResult> ApproveRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.FindById(id);
                var admin = _userManager.GetUserAsync(User).Result;
                var leaveAllocation = await _leaveAllocationRepository.GetLeaveAllocationByEmployeeAndLeaveType(leaveRequest.RequestingEmployeeId, leaveRequest.Id);

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = admin.Id;
                leaveRequest.DateActioned = DateTime.Now;

                int dayRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                leaveAllocation.NumberOfDays -= dayRequested;

                await _leaveRequestRepository.Update(leaveRequest);
                await _leaveAllocationRepository.Update(leaveAllocation);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        public async Task<ActionResult> RejectRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.FindById(id);
                var admin = _userManager.GetUserAsync(User).Result;
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = admin.Id;
                leaveRequest.DateActioned = DateTime.Now;

                await _leaveRequestRepository.Update(leaveRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: LeaveRequestController/Create
        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _leaveTypeRepository.FindAll();
            var leaveTypeItems = leaveTypes.Select(lt => new SelectListItem 
            {
                Text = lt.Name,
                Value = lt.Id.ToString()
            });

            var createLeaveRequestVM = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems
            };
            return View(createLeaveRequestVM);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestVM createLeaveRequestVM)
        {
            try
            {
                var startDate = Convert.ToDateTime(createLeaveRequestVM.StartDate);
                var endDate = Convert.ToDateTime(createLeaveRequestVM.EndDate);

                var leaveTypes = await _leaveTypeRepository.FindAll();
                var leaveTypeItems = leaveTypes.Select(lt => new SelectListItem
                {
                    Text = lt.Name,
                    Value = lt.Id.ToString()
                });

                createLeaveRequestVM.LeaveTypes = leaveTypeItems;

                if (!ModelState.IsValid)
                {
                    return View(createLeaveRequestVM);
                }

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("","Start Date cannot be further in the future than End Date");
                    return View(createLeaveRequestVM);
                }


                var employee = _userManager.GetUserAsync(User).Result;
                var leaveAllocation = await _leaveAllocationRepository.GetLeaveAllocationByEmployeeAndLeaveType(employee.Id, createLeaveRequestVM.LeaveTypeId);

                var dayRequested = (int)(endDate.Date - startDate.Date).TotalDays;
                if (dayRequested > leaveAllocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You do not sufficient days for this request");
                    return View(createLeaveRequestVM);
                }

                var leaveRequestVM = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = createLeaveRequestVM.LeaveTypeId,
                    RequestComments = createLeaveRequestVM.RequestComments
                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestVM);

                bool isSuccess = await _leaveRequestRepository.Create(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong with submitting your record");
                    return View(createLeaveRequestVM);
                }

                return RedirectToAction(nameof(MyLeave));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(createLeaveRequestVM);
            }
        }

        // GET: LeaveRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: LeaveRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Delete/5
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


        public async Task<ActionResult> MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;

            var employeeLeaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationByEmployee(employee.Id);
            var employeeLeaveAllocationsVM = _mapper.Map<List<LeaveAllocationVM>>(employeeLeaveAllocations);

            var employeeLeaveRequests = await _leaveRequestRepository.GetLeaveRequestsByEmployee(employee.Id);
            var employeeLeaveRequestsVM = _mapper.Map<List<LeaveRequestVM>>(employeeLeaveRequests);

            var employeeRequestVM = new EmployeeRequestVM
            {
                LeaveAllocations = employeeLeaveAllocationsVM,
                LeaveRequests = employeeLeaveRequestsVM
            };

            return View(employeeRequestVM);
        }

        public async Task<ActionResult> CancelRequest(int id)
        {
            var leaveRequest = await _leaveRequestRepository.FindById(id);
            leaveRequest.Cancelled = true;
            await _leaveRequestRepository.Update(leaveRequest);
            return RedirectToAction(nameof(MyLeave));
        }
    }
}
