using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Repositories;
using test_kumo_eip0001repositories;

namespace test_kumo_eip0001application
{
    public class AccountService
    {
       
        private readonly AccountRepository accountRepository;
        public AccountService()
        {
            accountRepository = new AccountRepository();
        }
        public void DeleteAccount(params Account[] accounts)
        {
            accountRepository.DeleteDeferred(accounts);
        }

        public void AddAccount(params Account[] accounts)
        {
            accountRepository.AddDeferred(accounts);
        }

        public void UpdateAccount(params Account[] accounts)
        {
            accountRepository.UpdateDeferred(accounts);
        }

        public IQueryable<Account> GetAll()
        {
            return accountRepository.GetAll();
        }

        public IQueryable<User> GetAllUsers()
        {
            return accountRepository.GetAllUsers();
        }

        public Account GetById(int id)
        {
            return accountRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
        }

        public User GetUserById(string id)
        {
            return accountRepository.GetAllUsers()
                .Where(x => x.Id.ToLower() == id.ToLower())
                .FirstOrDefault();
        }

        public AspNetUser GetAspNetUserById(string id)
        {
            return accountRepository.GetAllAspNetUsers()
                .Where(x => x.Id.ToLower() == id.ToLower())
                .FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return accountRepository.GetAllUsers().Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public void DeleteUser(params User[] users)
        {
            List<string> ids = users.Select(u => u.Id).ToList();
            var accounts = accountRepository.GetAllAspNetUsers()
                .Where(x => ids.Any(id => x.Id.ToLower() == id.ToLower()))
                .ToArray();
            accountRepository.DeleteDeferred(accounts);
        }

        public void UpdateUser(params User[] users)
        {
            List<string> ids = users.Select(u => u.Id).ToList();
            var accounts = accountRepository.GetAllAspNetUsers()
                .Where(x => ids.Any(id => x.Id.ToLower() == id.ToLower()))
                .ToList();

            accounts.ForEach(item =>
            {
                var user = users.Where(u => u.Id.ToLower() == item.Id.ToLower()).FirstOrDefault();
                item.Firstname = user.Firstname;
                item.Lastname = user.Lastname;
                item.PhoneNumber = user.PhoneNumber;
                item.Company = user.Company;
            });

            accountRepository.UpdateDeferred(accounts.ToArray());
        }

    }
}
