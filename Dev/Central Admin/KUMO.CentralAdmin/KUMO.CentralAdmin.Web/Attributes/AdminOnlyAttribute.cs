using KUMO.CentralAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KUMO.CentralAdmin.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AdminOnlyAttribute : AuthorizeAttribute
    {

        public AdminOnlyAttribute()
        {

            Roles = RoleName.Admin;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Account", action = "Login" }));
            }
            else if (!filterContext.HttpContext.User.IsInRole(Roles))
            {
                filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
            }
        }
    } 
}