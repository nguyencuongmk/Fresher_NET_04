using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public LeaveTypeController(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        // GET: LeaveTypeController
        public async Task<ActionResult> Index()
        {
            var leaveTypes = await _leaveTypeRepository.FindAll();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leaveTypes.ToList());
            return View(model);
        }

        // GET: LeaveTypeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var isExists = await _leaveTypeRepository.IsExists(id);
            if (!isExists)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypeRepository.FindById(id);
            var leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);

            return View(leaveTypeVM);
        }

        [Authorize(Roles ="Administrator")]
        // GET: LeaveTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(leaveTypeVM);
                }

                var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
                leaveType.DateCreated = DateTime.Now;

                var isSuccess = await _leaveTypeRepository.Create(leaveType);
                if (!isSuccess)
                {
                    ModelState.AddModelError("","Something went wrong...");
                    return View(leaveTypeVM);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong...");
                return View(leaveTypeVM);
            }
        }

        [Authorize(Roles = "Administrator")]
        // GET: LeaveTypeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var isExists = await _leaveTypeRepository.IsExists(id);
            if (!isExists)
            {
                return NotFound();
            }
            var leaveType = await _leaveTypeRepository.FindById(id);
            var leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);
            return View(leaveTypeVM);
        }


        [Authorize(Roles = "Administrator")]
        // POST: LeaveTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(leaveTypeVM);
                }

                var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
                var isSuccess = await _leaveTypeRepository.Update(leaveType);
                    
                if (!isSuccess)
                {
                    ModelState.AddModelError("","Something went wrong...");
                    return View(leaveTypeVM);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong...");
                return View(leaveTypeVM);
            }
        }

        [Authorize(Roles = "Administrator")]
        // GET: LeaveTypeController/Delete/5
        public async Task<ActionResult> Delete(/*LeaveTypeVM leaveTypeVM*/int id)
        {
            //----------Solution 1------------//
            //if (leaveTypeVM != null)
            //{
            //    var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
            //    var isSucess = _leaveTypeRepository.Delete(leaveType);
            //    if (!isSucess)
            //    {
            //        ModelState.AddModelError("", "Something went wrong...");
            //        return View();
            //    }
            //}
            //return RedirectToAction(nameof(Index));

            //----------Solution 2------------//
            var leaveType = await _leaveTypeRepository.FindById(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var isSuccess = await _leaveTypeRepository.Delete(leaveType);
            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));

            //----------Solution 3------------//
            //if (!_leaveTypeRepository.isExists(id))
            //{
            //    return NotFound();
            //}
            //var leaveType = _leaveTypeRepository.FindById(id);
            //var leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);
            //return View(leaveTypeVM);
        }


        [Authorize(Roles = "Administrator")]
        // POST: LeaveTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, LeaveTypeVM leaveTypeVM)
        {
            try
            {
                var leaveType = await _leaveTypeRepository.FindById(id);
                if (leaveType == null)
                {
                    return NotFound();
                }

                var isSuccess =  await _leaveTypeRepository.Delete(leaveType);
                if (!isSuccess)
                {
                    return View(leaveTypeVM);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(leaveTypeVM);
            }
        }
    }
}
