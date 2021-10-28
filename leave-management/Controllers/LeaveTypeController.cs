using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
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
        public ActionResult Index()
        {
            var leaveTypes = _leaveTypeRepository.FindAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leaveTypes);
            return View(model);
        }

        // GET: LeaveTypeController/Details/5
        public ActionResult Details(int id)
        {
            if (!_leaveTypeRepository.isExists(id))
            {
                return NotFound();
            }

            var leaveType = _leaveTypeRepository.FindById(id);
            var leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);

            return View(leaveTypeVM);
        }

        // GET: LeaveTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(leaveTypeVM);
                }

                var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
                leaveType.DateCreated = DateTime.Now;

                var isSuccess = _leaveTypeRepository.Create(leaveType);
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

        // GET: LeaveTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!_leaveTypeRepository.isExists(id))
            {
                return NotFound();
            }
            var leaveType = _leaveTypeRepository.FindById(id);
            var leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);
            return View(leaveTypeVM);
        }

        // POST: LeaveTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(leaveTypeVM);
                }

                var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
                var isSuccess = _leaveTypeRepository.Update(leaveType);

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

        // GET: LeaveTypeController/Delete/5
        public ActionResult Delete(/*LeaveTypeVM leaveTypeVM*/int id)
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
            var leaveType = _leaveTypeRepository.FindById(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var isSuccess = _leaveTypeRepository.Delete(leaveType);
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

        // POST: LeaveTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LeaveTypeVM leaveTypeVM)
        {
            try
            {
                var leaveType = _leaveTypeRepository.FindById(id);
                if (leaveType == null)
                {
                    return NotFound();
                }

                var isSuccess = _leaveTypeRepository.Delete(leaveType);
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
