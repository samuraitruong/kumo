using KUMO.CentralAdmin.Application;
using KUMO.CentralAdmin.Model;
using KUMO.CentralAdmin.Model.ViewModels;
using KUMO.CentralAdmin.Web.Mailers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KUMO.CentralAdmin.Web.Controllers
{
    
    public class TrialController : Controller
    {
        
        public ActionResult Index()
        {
           
            //throw new Exception("Run time Error");
            return PartialView();
           
        }

        [Route("Confirm")]
        public ActionResult Confirm()
        {

            return PartialView();

        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "Firstname,Lastname,Email,Phone,Company")]TrialUserViewModel user, string Firstname)
        {
            if (ModelState.IsValid)
            {
                var service = new EIPUserService();

                var existingUser = service.GetAll().FirstOrDefault(p => p.Email == user.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already exist.");
                    return PartialView(user);
                }

                //user. = DateTime.Now;

                EIPUser eip = new EIPUser();
                eip.CopyFrom<EIPUser>(user);
                eip.Type = EIPUserTypes.Trial.ToString();
                eip.CreatedDate = DateTime.Now;
                eip.IsAdmin = true;
                service.Add(eip);
                //send email
                var mailer = new TrialUserMailer();
                var mailModel = new TrialUserConfirmationMailModel();
                mailModel.CopyFrom<TrialUserConfirmationMailModel>(user);


                mailer.Confirmation(user.Email, mailModel).Send();


               return RedirectToAction("Confirm");

            }
            return PartialView(user);

        }

    }
}