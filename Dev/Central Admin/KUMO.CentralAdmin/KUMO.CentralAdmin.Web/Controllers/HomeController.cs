﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KUMO.CentralAdmin.Web.Controllers
{
    [Authorize]
    [AdminOnly]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}