using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model.ViewModels
{
    public class CalendarViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(ResourceType = typeof(ERPResource), Name = "JobScope")]
        public string JobScope { get; set; }

        [Required]
        [Display(ResourceType = typeof(ERPResource), Name = "Priority")]
        public string Priority { get; set; }

        [Required]
        [CompareValues("DueDate", CompareValues.LessThanOrEqualTo, ErrorMessage = "Start Date must be less than or equal Due Date")]
        [Display(ResourceType = typeof(ERPResource), Name = "StartDate")]
        public System.DateTime StartDate { get; set; }

        [Required]
        [Display(ResourceType = typeof(ERPResource), Name = "DueDate")]
        public System.DateTime DueDate { get; set; }

        [Display(ResourceType = typeof(ERPResource), Name = "CompletionDate")]
        public Nullable<System.DateTime> CompletedDate { get; set; }

        [Required]
        [Display(ResourceType = typeof(ERPResource), Name = "AssignedTo")]
        public string AssignedTo { get; set; }

        [Required]
        [Display(ResourceType = typeof(ERPResource), Name = "Status")]
        public string Status { get; set; }


        [Display(ResourceType = typeof(ERPResource), Name = "CompletePercent")]
        public decimal CompletedPercent { get; set; }

    }
}
