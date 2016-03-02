using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KUMO.CentralAdmin.Web.Mailers
{
    public class WelcomeTrialUserEmail : Email
    {
        public string To { get; set; }
        public string UserName { get; set; }
        
        public string SiteURL { get; set; }
        public string Firstname { get; set; }
        public string Password { get; set; }
    }
}