using System;
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
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001web.Mailers;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Utility;
using Microsoft.AspNet.Identity.EntityFramework;
using test_kumo_eip0001repositories.Model;


namespace test_kumo_eip0001web.Controllers
{
    public class KumoBaseController : Controller
    {
        public bool IsDlg{
            get
            {

                return (Request["isdlg"] != null && Request["isdlg"] == "1");
            }
        }

        public ActionResult ClosePopup()
        {
            return View("ClosePopup");
        }
        public async Task<ActionResult> ClosePopupAsync()
        {
            return  View("ClosePopup");
        }
        public KumoBaseController()
        {
            if (!string.IsNullOrEmpty(SessionManager.PrivateTenantConnectionString))
            {
                //create user manager from session
                var db = ApplicationDbContext.Create(SessionManager.PrivateTenantConnectionString);

                UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            }
        }
        public KumoBaseController(ApplicationUserManager userManager)
        {
            if (!string.IsNullOrEmpty(SessionManager.PrivateTenantConnectionString))
            {
                //create user manager from session
                var db = ApplicationDbContext.Create(SessionManager.PrivateTenantConnectionString);

                UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            }
           // UserManager = userManager;
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {

                _userManager = value;
            }
        }
        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            string actionName = (string)filterContext.RouteData.Values["action"];

            if (actionName == "Login")
            {
                return;
                
            }
            if (filterContext.Exception is InvalidTenantException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = Redirect("/Account/Logoff");

            }
            else

                if (filterContext.Exception is CAServiceException)
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = Redirect("/Account/Logoff");

                    //throw new Exception("Thinking about the logic here when CA site down. In the meantime, when CA site down, system will not working because there no services for Tenant site.");
                }

                else
                {
                    base.OnException(filterContext);
                }
        }
        protected override void OnActionExecuted( ActionExecutedContext filterContext)
        {

            var action = filterContext.RouteData.GetRequiredString("action");

            if (action == "LogOff" || action == "Login")
            {
                base.OnActionExecuted(filterContext);
                return;
            }
            if (User.Identity.IsAuthenticated && 
                (SessionManager.CurrentUser == null || SessionManager.CurrentTenant == null))
            {
                //Resoleve itentity
                var connString = TenantHelper.GetConnectionString(User.Identity.Name);
                if (string.IsNullOrEmpty(connString))
                {
                    filterContext.Result = RedirectToAction("LogOff", "Account");
                    return;
                }

                SetCurrentUser(connString);
            }
            
            if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Status == UserStatuses.TemporaryPassword.ToString())
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                //Redirect user
                if (actionName != "ChangePassword")
                {
                    RedirectToAction("ChangePassword", "Manage");
                    filterContext.Result = RedirectToAction("ChangePassword", "Manage");
                }
            }

            SetUpParameters(SessionManager.CurrentUser.Email);
        }

        private void SetCurrentUser(string connString)
        {
            var db = ApplicationDbContext.Create(connString);

            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            var user = UserManager.FindByEmail(User.Identity.Name);
            SessionManager.PrivateTenantConnectionString = connString;
            SessionManager.CurrentUser = user;
        }

        private void SetUpParameters(string email)
        {
            if (SessionManager.CurrentTenant != null && !string.IsNullOrEmpty(SessionManager.CurrentTenant.Components))
            {
                SessionManager.CurrentComponents = SessionManager.CurrentTenant.Components.Split(new char[] { ',' }).ToList();
            }

            if (SessionManager.Components == null || SessionManager.Components.Count <= 0)
            {
                var componentService = new ComponentService();
                SessionManager.Components = componentService.GetAll().ToList();
            }

            var userActionService = new UserActionService();
            if (User.IsInRole(RoleNames.Admin))
            {
                var perms = new List<UserAction>();
                var actions = userActionService.GetSystemActions();
                SessionManager.CurrentComponents.ForEach(c => {
                    actions.ForEach(a => {
                        perms.Add(new UserAction
                        { 
                            ComponentId = int.Parse(c),
                            Component = SessionManager.Components.FirstOrDefault(x => x.Id == int.Parse(c)),
                            ActionId = a.Id,
                            UserId = SessionManager.CurrentUser.Id
                        });
                    });
                });

                SessionManager.CurrentUserPerm = perms;
            }
            else
            {
                SessionManager.CurrentUserPerm = userActionService.GetUserActions(email);
            }
        }
    }
}