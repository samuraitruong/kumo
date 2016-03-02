using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Resources;
using test_kumo_eip0001repositories;

namespace test_kumo_eip0001application
{
    public class EmployeeService :ServiceBase<Employee>
    {
        public EmployeeService()
        {
        }


        public IEnumerable<object> GetEmployeeStatus()
        {
            return new[] { 
                new  {Name= StringResources.EmployeeStatus_Current, Id=StringResources.EmployeeStatus_Current},
                new  {Name=StringResources.EmployeeStatus_Fired, Id=StringResources.EmployeeStatus_Fired},
                new  {Name=StringResources.EmployeeStatus_Resigned, Id=StringResources.EmployeeStatus_Resigned}
            };
        }
    }
}
