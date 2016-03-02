using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Application
{
    public class TestMe
    {
        public void Test()
        {
            throw new Exception(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data"));
        }
    }
}
