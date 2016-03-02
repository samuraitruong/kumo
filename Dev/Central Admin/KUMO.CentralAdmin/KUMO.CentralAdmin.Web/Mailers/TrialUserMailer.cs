using KUMO.CentralAdmin.Model.ViewModels;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KUMO.CentralAdmin.Web.Mailers
{
    public class TrialUserMailer : MailerBase
    {
        public TrialUserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Confirmation(string to, TrialUserConfirmationMailModel model)
        {
            //ViewBag.Data = someObject;
            ViewBag.Model = model;

            return Populate(x =>
            {
                x.Subject = "Your KUMO  trial Account";
                x.ViewName = "Confirmation";
                x.To.Add(to);
            });
        }

        

    }
}