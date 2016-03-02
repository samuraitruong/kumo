using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;

namespace test_kumo_eip0001repositories
{
    public class UserActionRepository : Repository<UserAction>
    {
        public List<UserAction> GetUserActions(string email)
        {
            var efObject = ContextFactory.GetCurrent().Set<AspNetUser>()
                .Include("UserActions")
                .FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

            if (efObject != null)
            {
                return efObject.UserActions.ToList();
            }

            return new List<UserAction>();
        }

        public IQueryable<SystemAction> GetSystemActions()
        {
            return ContextFactory.GetCurrent().Set<SystemAction>();
        }
    }
}
