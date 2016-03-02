using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model.ViewModels
{
    public class TaskTrackerViewModel 
    {
        public int Id { get; set; }
        [Required]
        [Display(ResourceType = typeof(TaskTrackerResource), Name = "TaskName")]
        public string TaskName { get; set; }

        [Required]
        [Display(ResourceType = typeof(TaskTrackerResource), Name = "Priority")]       
        public string Priority { get; set; }

       

         [Required]
         [Display(ResourceType = typeof(TaskTrackerResource), Name = "StartDate")]     
        public System.DateTime StartDate { get; set; }

         [Required]
         [Display(ResourceType = typeof(TaskTrackerResource), Name = "DueDate")]   
        public System.DateTime DueDate { get; set; }

        [Display(ResourceType = typeof(TaskTrackerResource), Name = "CompletionDate")]   
        public Nullable<System.DateTime> CompletedDate { get; set; }

        [Required]
        [Display(ResourceType = typeof(TaskTrackerResource), Name = "AssignedTo")]   
        public string AssignedTo { get; set; }
               

        [Required]
        [Display(ResourceType = typeof(TaskTrackerResource), Name = "TaskStatus")]
        public string Status { get; set; }

        [Required]
        [Display(ResourceType = typeof(TaskTrackerResource), Name = "Description")]   
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

         [Required]
         [Display(ResourceType = typeof(TaskTrackerResource), Name = "CompletePercent")]
        public decimal CompletedPercent { get; set; }


    }
}
