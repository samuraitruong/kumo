using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;

namespace test_kumo_eip0001application
{
    public static class SecurityService
    {
        public static void CheckPermission(List<UserAction> actions, UserAction[] permissionNeeded)
        {
            if (!DoesPersonHavePermission(actions, permissionNeeded))
            {
                throw new SecurityException("User doesn't have permission", new Exception("User doesn't have permission"));
            }
        }

        public static bool DoesPersonHavePermission(List<UserAction> actions, UserAction[] permissionNeeded)
        {

            if (actions.Count == 0)
                return false;

            return permissionNeeded.Any(action => actions.Any(x => x.ActionId == action.ActionId && x.ComponentId == action.ComponentId));
        }
    }
}
