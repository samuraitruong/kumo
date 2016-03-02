using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;

namespace test_kumo_eip0001application
{
    public class ComponentService : ServiceBase<Component>
    {
        public test_kumo_eip0001model.Component GetComponentByName(string name)
        {
            return GetAll().Where(x => x.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();
        }
    }
}
