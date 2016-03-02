using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using test_kumo_eip0001web.Models;
using test_kumo_eip0001application;
using System.Web.Security;
using test_kumo_eip0001model;

using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001web.Mailers;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Utility;



namespace test_kumo_eip0001web.Controllers
{
    [AllowAnonymous]
    public class UserAPIController : System.Web.Http.ApiController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                var abstractContext = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);

                return _userManager ?? abstractContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
    {

                _userManager = value;
            }
        }

        [HttpGet]
        [Authorize]
        public bool IsAdmin()
        {
            var isAdmin = false;

            if (UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.Admin)) /*This is True!!*/
            {
                isAdmin = true;
            }
            return isAdmin;
            //return  System.Web.HttpContext.Current.User.IsInRole(RoleNames.Admin);
        }
    }
}
