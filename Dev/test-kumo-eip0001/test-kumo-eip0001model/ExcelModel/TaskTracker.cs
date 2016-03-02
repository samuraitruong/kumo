using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model
{
    [MetadataType(typeof(TaskTrackerMetadata))]
    public partial class TaskTracker 
    {
       
        [DisplayName("Assign To")]
        
        
        public string _AssignTo
        {
            get { return Assignee.Firstname + " " + Assignee.Lastname; }
        }

        //THis method to return the order/formated field for auto export function
        public static string ExcelFields()
        {

            return "TaskName;_AssignTo;Priority;StartDate";
        }
    }

    public class TaskTrackerMetadata
    {
        [ExcelIgnored]
        public string AssignedTo { get; set; }

    }
}
