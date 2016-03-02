using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : ActionFilterAttribute
    {
        public test_kumo_eip0001model.Actions Action { get; set; }
        public string Component { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SessionManager.CurrentUserPerm != null && SessionManager.CurrentUserPerm.Count > 0)
            {
                List<UserAction> actions = new List<UserAction>();
                var currentComponent = SessionManager.Components.FirstOrDefault(c => c.Name == Component);
                actions.Add(new UserAction { ComponentId = currentComponent.Id, ActionId = (int)Action });

                if (!SecurityService.DoesPersonHavePermission(SessionManager.CurrentUserPerm, actions.ToArray()))
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