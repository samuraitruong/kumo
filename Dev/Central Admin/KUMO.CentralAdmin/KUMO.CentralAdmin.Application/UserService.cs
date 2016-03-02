using KUMO.CentralAdmin.Model;
using KUMO.CentralAdmin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Application
{
    public class UserService
    {
        private readonly UserRepository repository;
        public UserService()
        {
            repository = new UserRepository();
        }

        public IQueryable<User> GetAllUsers()
        {
            return repository.GetAllUsers();
        }

        public User GetUserById(string id)
        {
            return repository.GetAllUsers().Where(x => x.Id.ToLower() == id.ToLower()).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return repository.GetAllUsers().Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public void DeleteUser(params User[] users)
        {
            List<string> ids = users.Select(u => u.Id).ToList();
            var accounts = repository.GetAllAspNetUsers()
                .Where(x => ids.Any(id => x.Id.ToLower() == id.ToLower()))
                .ToArray();
            repository.DeleteDeferred(accounts);
        }

        public void UpdateUser(params User[] users)
        {
            List<string> ids = users.Select(u => u.Id).ToList();
            var accounts = repository.GetAllAspNetUsers()
                .Where(x => ids.Any(id => x.Id.ToLower() == id.ToLower()))
                .ToList();

            accounts.ForEach(item =>
            {
                var user = users.Where(u => u.Id.ToLower() == item.Id.ToLower()).FirstOrDefault();
                item.FirstName = user.Firstname;
                item.LastName = user.Lastname;
                item.PhoneNumber = user.PhoneNumber;
                item.Company = user.Company;
            });

            repository.UpdateDeferred(accounts.ToArray());
        }
    }
}
