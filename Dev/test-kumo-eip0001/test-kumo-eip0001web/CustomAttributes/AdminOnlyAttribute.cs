using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test_kumo_eip0001model;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SessionManager.CurrentUser == null)
            {
                filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
            }
            else
            {
                var accountService = new test_kumo_eip0001application.AccountService();
                var aspNetUser = accountService.GetAspNetUserById(SessionManager.CurrentUser.Id);

                if (!aspNetUser.AspNetRoles.Any(nr => nr.Name == RoleNames.Admin))
                {
                    filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
                }
            }
        }
    }
}