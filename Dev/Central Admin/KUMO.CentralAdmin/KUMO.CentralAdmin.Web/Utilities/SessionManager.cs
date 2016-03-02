using KUMO.CentralAdmin.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public class SessionManager
    {
        public static string CURRENT_USER_SESSION_NAME = "CURRENT_USER_SESSION_NAME" + HttpContext.Current.User.Identity.Name;
        public static string CURRENT_COMPANY_SESSION_NAME = "CURRENT_COMPANY_SESSION_NAME" + HttpContext.Current.User.Identity.Name;

        public static ApplicationUser CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[CURRENT_USER_SESSION_NAME] != null)
                {
                    return HttpContext.Current.Session[CURRENT_USER_SESSION_NAME] as ApplicationUser;
                }
                return new ApplicationUser();
            }

            set { HttpContext.Current.Session[CURRENT_USER_SESSION_NAME] = value; }
        }

       
    }
}