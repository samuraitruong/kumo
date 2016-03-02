using System;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Repositories;

namespace test_kumo_eip0001repositories
{
    public class AccountRepository : Repository<Account>
    {
        public AccountRepository() 
            : base()
        {
        }

        public IQueryable<User> GetAllUsers()
        {
            return ContextFactory.GetCurrent().Set<AspNetUser>().Select(u => new User()
            {
                UserName = u.UserName,
                Email = u.Email,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
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
