using KUMO.CentralAdmin.Model.ViewModels;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KUMO.CentralAdmin.Web.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome(string to, WelcomeMailViewModel model)
        {
            //ViewBag.Data = someObject;
            ViewBag.Model = model;


            return Populate(x =>
            {
                x.Subject = "Your KUMO Account Information";
                x.ViewName = "Welcome";
                x.To.Add(to);
            });
        }

        public virtual MvcMailMessage PasswordReset(string to, PasswordResetViewModel model)
        {
            //ViewBag.Data = someObject;
            ViewBag.Model = model;
            return Populate(x =>
            {
                x.Subject = "Reset Your KUMO Password";
                x.ViewName = "PasswordReset";
                x.To.Add(to);
            });
        }

    }
}