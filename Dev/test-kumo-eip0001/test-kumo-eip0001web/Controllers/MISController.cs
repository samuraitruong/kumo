using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test_kumo_eip0001application;
using test_kumo_eip0001model;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]
    [RedirectingAction(Action = "MIS")]
    public class MISController : KumoBaseController
    {
        public MISController()
        {
            ViewBag.LogoUrl = "/Content/images/MISLogo.jpg";
            ViewBag.Url = "/mis";

        }
        // GET: MIS
        [Permission(Component = "MIS", Action = Actions.View)]
        public ActionResult Index()
        {
            DocumentLibraryService service = new DocumentLibraryService();

            return View(service.GetAll().ToList());
        }
    }
}