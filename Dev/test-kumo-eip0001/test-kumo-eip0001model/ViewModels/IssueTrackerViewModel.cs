using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model.ViewModels
{
    public class IssueTrackerViewModel 
    {
        public int Id { get; set; }
        [Required]
        [Display(ResourceType = typeof(IssueTrackerResource), Name = "IssueName")]
        public string Name { get; set; }

        [Required]
        [Display(ResourceType = typeof(IssueTrackerResource), Name = "Priority")]       
        public string Priority { get; set; }

         [Required]
         [Display(ResourceType = typeof(IssueTrackerResource), Name = "Category")]     
        public string Category { get; set; }

         [Required]
         [Display(ResourceType = typeof(IssueTrackerResource), Name = "StartDate")]     
        public System.DateTime StartDate { get; set; }

         [Required]
         [Display(ResourceType = typeof(IssueTrackerResource), Name = "DueDate")]   
        public System.DateTime DueDate { get; set; }

        [Display(ResourceType = typeof(IssueTrackerResource), Name = "ClosedDate")]   
        public Nullable<System.DateTime> ClosedDate { get; set; }

        [Required]
        [Display(ResourceType = typeof(IssueTrackerResource), Name = "AssignedTo")]   
        public string AssignedTo { get; set; }

        [Required]
        [Display(ResourceType = typeof(IssueTrackerResource), Name = "IssueStatus")]
        public string Status { get; set; }

        [Required]
        [Display(ResourceType = typeof(IssueTrackerResource), Name = "Description")]   
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(ResourceType = typeof(IssueTrackerResource), Name = "Resolution")]
        [DataType(DataType.MultilineText)]
        public string Resolution { get; set; }

    }
}
