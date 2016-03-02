using System.Web.Mvc;

namespace Kumo.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        
        public ActionResult Home()
        {
            ViewBag.Title = "Home Page";

            return PartialView();
        }
    }
}