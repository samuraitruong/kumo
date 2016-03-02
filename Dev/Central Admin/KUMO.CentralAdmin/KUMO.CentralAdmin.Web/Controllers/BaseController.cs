using KUMO.CentralAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace KUMO.CentralAdmin.Web
{
    public class BaseController : Controller
    {
        public BaseController() { }
        public BaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        protected override void OnActionExecuted( ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (User.Identity.IsAuthenticated && SessionManager.CurrentUser == null)
            {
                var user = UserManager.FindByEmail(User.Identity.Name);
                //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                SessionManager.CurrentUser = user;
            }
            //
            if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Status == UserStatus.TemporaryPassword.ToString())
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                //Redirect user
                if (actionName != "ChangePassword")
                {
                    RedirectToAction("ChangePassword", "Manage");
                    filterContext.Result = RedirectToAction("ChangePassword", "Manage");
                }
                //else
                //{
                //    base.OnActionExecuted(filterContext);
                //}
            }
            //else
            //{
            //    base.OnActionExecuted(filterContext);
            //}
        }
    }
}