using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001repositories.DataModel
{
    public class InvalidTenantException : Exception
    {
        public InvalidTenantException()
            : base("Invalid tenant connection string")
        {

        }
    }
}
