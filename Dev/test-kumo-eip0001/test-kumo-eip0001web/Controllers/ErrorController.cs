using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace test_kumo_eip0001web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.Message = "Error";
            ViewBag.Description = "An error occurred while processing your request.";
            return View();
        }

        public ActionResult AccessDenied()
        {
            ViewBag.Message = "Access Denied";
            ViewBag.Description = "You do not have access to view this page.";
            return View();
        }
    }
}