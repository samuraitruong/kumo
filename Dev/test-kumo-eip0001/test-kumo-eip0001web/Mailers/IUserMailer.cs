using Mvc.Mailer;
using test_kumo_eip0001model;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Models;

namespace test_kumo_eip0001web.Mailers
{ 
    public interface IUserMailer
    {
        MvcMailMessage Welcome(string to, WelcomeMailViewModel model);
        MvcMailMessage PasswordReset(string to, PasswordResetViewModel model);
	}
}