﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace leave_management.Models
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }

        public EmployeeVM RequestingEmployee { get; set; }

        [Display(Name = "Employee Name")]
        public string RequestingEmployeeId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public LeaveTypeVM LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        public bool Cancelled { get; set; }

        [Display(Name = "Employee Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

        [Display(Name = "Date Actioned")]
        public DateTime DateActioned { get; set; }

        [Display(Name = "Approval Status")]
        public bool? Approved { get; set; }

        public EmployeeVM ApprovedBy { get; set; }

        [Display(Name = "Approver Date")]
        public string ApprovedById { get; set; }
    }

    public class AdminLeaveRequestVM
    {
        [Display(Name = "Total Requests")]
        public int TotalRequests { get; set; }

        [Display(Name = "Approved Requests")]
        public int ApprovedRequests { get; set; }

        [Display(Name = "Pending Requests")]
        public int PendingRequests { get; set; }

        [Display(Name = "Reject Requests")]
        public int RejectRequests { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }

    public class CreateLeaveRequestVM
    {
        [Required]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }

        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Employee Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }
    }

    public class EmployeeRequestVM
    {
        public List<LeaveAllocationVM> LeaveAllocations { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }
}