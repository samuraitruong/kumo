using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
using test_kumo_eip0001web.Models;

namespace test_kumo_eip0001web.Controllers
{
    [ApiAdminOnly]
    public class AccountsAPIController : ApiController
    {
        private AccountService accountService = new AccountService();

        [AdminOnly]
        // GET: _api/AccountsAPI
        public IQueryable<User> GetUsers()
        {
            return accountService.GetAllUsers();
        }

        // GET: _api/AccountsAPI/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult GetAccount(int id)
        {
            Account account = accountService.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: _api/AccountsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccount(int id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Id)
            {
                return BadRequest();
            }

            try
            {
                accountService.UpdateAccount(account);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: _api/AccountsAPI
        [ResponseType(typeof(Account))]
        public IHttpActionResult PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            accountService.AddAccount(account);

            return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        }

        // DELETE: _api/AccountsAPI/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult DeleteAccount(int id)
        {
            Account account = accountService.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            accountService.DeleteAccount(account);

            return Ok(account);
        }

        private bool AccountExists(int id)
        {
            return accountService.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}