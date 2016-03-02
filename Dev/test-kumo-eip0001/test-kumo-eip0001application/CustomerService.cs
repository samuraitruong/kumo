using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Repositories;
using test_kumo_eip0001repositories;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001application
{
    public class CustomerService : ServiceBase<Customer>
    {
        public CustomerService()
        {
        }

        public List<string> GetLeadTypes()
        {
            List<string> types = new List<string>() 
            { 
                CRMResource.CustomerType, 
                CRMResource.InvalidType, 
                CRMResource.PotentialType 
            };
            return types;
        }
    }
}
