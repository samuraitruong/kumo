using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test_kumo_eip0001model;
using test_kumo_eip0001web.Utility;

namespace System.Web.Mvc.Html
{
    public static  class HtmlHelperExtension
    {
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, bool htmlEncode = true)
        {
            var tagActionLink = htmlHelper.ActionLink("[__replace__]", actionName, controllerName, routeValues, htmlAttributes).ToHtmlString();
            if (htmlEncode == false)
            {
                return MvcHtmlString.Create(tagActionLink.Replace("[__replace__]", linkText));
            }
            return  MvcHtmlString.Create(tagActionLink);
        }

        public static MvcHtmlString RouteLink(this HtmlHelper helper, DocumentLibrary library)
        {

            return helper.RouteLink(library.Name, KumoConstants.DOCUMENT_ROOT_ROUTE_NAME, new { doclib = library.Name, id = library.Id, action = "index" });
        }

    }
}