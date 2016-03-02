using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Repositories
{
    public interface IContextFactory
    {
        EntityContext GetCurrent();
        void SetUp();
    }
}
