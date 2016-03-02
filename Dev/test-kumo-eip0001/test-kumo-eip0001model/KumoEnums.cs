using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model
{
    public enum UserStatuses
    {
        Normal,
        Locked,
        TemporaryPassword        
    }

    public enum DocumentType
    {
        Folder = 1,
        File = 2
    }
}
