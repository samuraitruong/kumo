using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001repositories.DataModel;

namespace test_kumo_eip0001repositories
{
    public interface IContextFactory
    {
        EntityContext GetCurrent();
        void SetUp();
    }
}
