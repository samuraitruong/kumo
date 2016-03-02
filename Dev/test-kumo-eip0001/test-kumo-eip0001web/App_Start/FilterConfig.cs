using System.Web;
using System.Web.Mvc;
using test_kumo_eip0001web.CustomAttributes;

namespace test_kumo_eip0001web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitializeSimpleMembershipAttribute());
        }
    }
}
