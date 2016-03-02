using KUMO.CentralAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace KUMO.CentralAdmin.Repositories
{
    public class UserRepository : Repository<AspNetUser>
    {
        public UserRepository() 
            : base()
        {
        }

        public IQueryable<User> GetAllUsers()
        {
            return ContextFactory.GetCurrent().Set<AspNetUser>().Select(u => new User()
            {
                UserName = u.UserName,
                Email = u.Email,
                Firstname = u.FirstName,
                Lastname = u.LastName,
                Company = u.Company,
                Id = u.Id,
                PhoneNumber = u.PhoneNumber
            });
        }

        public IQueryable<AspNetUser> GetAllAspNetUsers()
        {
            return ContextFactory.GetCurrent().Set<AspNetUser>();
        }

        public virtual void DeleteDeferred(params AspNetUser[] items)
        {
            ContextFactory.GetCurrent().Set<AspNetUser>().RemoveRange(items.ToArray());
            CommitChanges();
        }

        public virtual void UpdateDeferred(params AspNetUser[] items)
        {
            ContextFactory.GetCurrent().Set<AspNetUser>().AddOrUpdate(items.ToArray());
            CommitChanges();
        }
    }
}
