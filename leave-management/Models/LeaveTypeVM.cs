using System;
using System.ComponentModel.DataAnnotations;

namespace leave_management.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Default number of days")]
        [Range(1,25,ErrorMessage ="Please enter a valid number")]
        public int DefaultDay { get; set; }

        [Display(Name="Date Created")]
        public DateTime DateCreated { get; set; }
    }
}