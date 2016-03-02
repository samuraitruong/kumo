using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web
{
    public class RedirectingActionAttribute : ActionFilterAttribute
    {
        public string Action { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SessionManager.CurrentComponents != null && SessionManager.CurrentComponents.Count > 0)
            {
                var componentService = new test_kumo_eip0001application.ComponentService();
                var currentComponent = componentService.GetComponentByName(Action);
                if (!SessionManager.CurrentComponents.Contains(currentComponent.Id.ToString()))
                {
                    filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
                }
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
            }
        }
    }
}