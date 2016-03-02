using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Repositories;

namespace test_kumo_eip0001repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository() 
            : base()
        {
        }

    }
}
