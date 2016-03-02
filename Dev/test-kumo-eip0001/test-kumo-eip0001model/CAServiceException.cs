using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001repositories.Model
{
    public class CAServiceException : Exception
    {
        public CAServiceException()
            : base("Central Administration services is unavailable. please contact site admin.")
        {

        }
    }
}
