using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test_kumo_eip0001model;
using test_kumo_eip0001web.Models;

namespace test_kumo_eip0001web.Utility
{
    

    public class SessionManager
    {
        public const string CURRENT_TENANT_SESSION_NAME = "CURRENT_TENANT_SESSION_NAME";
        public const string CURRENT_USER_SESSION_NAME = "CURRENT_USER_SESSION_NAME";
        public const string CURRENT_USER_PERM_SESSION_NAME = "CURRENT_USER_PERM_SESSION_NAME";
        public const string TENANT_CONNECTION_STRING_NAME = "TENANT_CONNECTION_STRING_NAME";
        public const string CURRENT_COMPONENT_SESSION_NAME = "CURRENT_COMPONENT_SESSION_NAME";
        public const string TENANT_COMPONENTS_NAME = "TENANT_COMPONENTS_NAME";
        public static string PrivateTenantConnectionString
        {
            get
            {
                if (HttpContext.Current.Session != null &&
                    HttpContext.Current.Session[TENANT_CONNECTION_STRING_NAME] != null)
                {
                    return HttpContext.Current.Session[TENANT_CONNECTION_STRING_NAME] as string;
                }
                
                if(HttpContext.Current != null &&
                    HttpContext.Current.User!= null &&
                    HttpContext.Current.User.Identity != null &&
                    HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var conn = TenantHelper.GetConnectionString(HttpContext.Current.User.Identity.Name);

                    if (!string.IsNullOrEmpty(conn))
                    {
                        HttpContext.Current.Session[TENANT_CONNECTION_STRING_NAME] = conn;
                        return conn;
                    }
                }
                return string.Empty;
            }

            set { HttpContext.Current.Session[TENANT_CONNECTION_STRING_NAME] = value; }
        }

        public static ApplicationUser CurrentUser
        {
            get
            {
                if(HttpContext.Current.Session[CURRENT_USER_SESSION_NAME] != null)
                {
                    return HttpContext.Current.Session[CURRENT_USER_SESSION_NAME] as ApplicationUser;
                }
                else
                {
                    if (HttpContext.Current != null &&
                    HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity != null &&
                    HttpContext.Current.User.Identity.IsAuthenticated)
                    {

                        var db = ApplicationDbContext.Create(PrivateTenantConnectionString);

                        var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                        var find = userMgr.FindByEmailAsync(HttpContext.Current.User.Identity.Name);
                        HttpContext.Current.Session[CURRENT_USER_SESSION_NAME] = find.Result;
                        return find.Result;

                    }


                }
               
                //return new ApplicationUser();
                return null;
            }

            set{HttpContext.Current.Session[CURRENT_USER_SESSION_NAME]  = value;}
        }

        public static List<UserAction> CurrentUserPerm
        {
            get
            {
                if (HttpContext.Current.Session[CURRENT_USER_PERM_SESSION_NAME] != null)
                {
                    return HttpContext.Current.Session[CURRENT_USER_PERM_SESSION_NAME] as List<UserAction>;
                }
                else
                {
                    return new List<UserAction>();
                }
            }

            set { HttpContext.Current.Session[CURRENT_USER_PERM_SESSION_NAME] = value; }
        }

        public static ClientInfo CurrentTenant
        {
            get
            {
                if (HttpContext.Current.Session[CURRENT_TENANT_SESSION_NAME] != null)
                {
                    return HttpContext.Current.Session[CURRENT_TENANT_SESSION_NAME] as ClientInfo;
                }
                else
                {
                    //Reload data form CA


                    var connString = PrivateTenantConnectionString;

                    return HttpContext.Current.Session[CURRENT_TENANT_SESSION_NAME] as ClientInfo;

                }
                //return new ApplicationUser();
            }

            set { HttpContext.Current.Session[CURRENT_TENANT_SESSION_NAME] = value; }
        }

        public static List<Component> Components
        {
            get
            {
                if (HttpContext.Current.Session[TENANT_COMPONENTS_NAME] != null)
                {
                    return HttpContext.Current.Session[TENANT_COMPONENTS_NAME] as List<Component>;
                }
                return new List<Component>();
            }

            set { HttpContext.Current.Session[TENANT_COMPONENTS_NAME] = value; }
        }

        public static List<string> CurrentComponents
        {
            get
            {
                if (HttpContext.Current.Session[CURRENT_COMPONENT_SESSION_NAME] != null)
                {
                    return HttpContext.Current.Session[CURRENT_COMPONENT_SESSION_NAME] as List<string>;
                }
                return null;
            }

            set { HttpContext.Current.Session[CURRENT_COMPONENT_SESSION_NAME] = value; }
        }

        
    }
}