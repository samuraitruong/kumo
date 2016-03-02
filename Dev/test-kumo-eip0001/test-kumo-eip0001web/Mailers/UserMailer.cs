using Mvc.Mailer;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Models;

namespace test_kumo_eip0001web.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
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