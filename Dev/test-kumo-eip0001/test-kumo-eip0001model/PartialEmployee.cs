using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model
{
    public partial class Employee
    {
        [Display(ResourceType = typeof(StringResources), Name = "EmployeeName")]
        public string Name
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
    }
}
