using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using test_kumo_eip0001model;

namespace test_kumo_eip0001web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAdminOnlyAttribute : AuthorizeAttribute
    {
        public ApiAdminOnlyAttribute()
        {

            Roles = RoleNames.Admin;

        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated || !HttpContext.Current.User.IsInRole(Roles))
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}