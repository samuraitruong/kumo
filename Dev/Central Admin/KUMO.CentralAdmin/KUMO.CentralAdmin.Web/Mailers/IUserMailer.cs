using KUMO.CentralAdmin.Model.ViewModels;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace KUMO.CentralAdmin.Web.Mailers
{
    public interface IUserMailer
    {
        MvcMailMessage Welcome(string to, WelcomeMailViewModel model);
        MvcMailMessage PasswordReset(string to, PasswordResetViewModel model);
    }
}