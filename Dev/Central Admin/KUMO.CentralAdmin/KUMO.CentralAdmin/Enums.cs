using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Model
{
    public enum UserStatus
    {
        Normal,
        Locked,
        TemporaryPassword
    }


    public enum ClientStatus
    {
        Active,
        Inactive,
        Deleted,
        Pending,
        Deploying,
        Deployed,
        Error,
        PendingTrialUser,
        TrialRegistered

    }
    public enum EIPUserTypes
    {
        Trial,
        Production
    }
}
