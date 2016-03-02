using KUMO.CentralAdmin.Application;
using KUMO.CentralAdmin.Model;
using KUMO.CentralAdmin.Model.ViewModels;
using KUMO.CentralAdmin.Web.Mailers;
using KUMO.CentralAdmin.Web.Models;
using KUMO.CentralAdmin.Web.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;
using System.Web.Hosting;
using Postal;
using System.IO;

namespace KUMO.CentralAdmin.Web.Utilities
{
    public class TrialUserHelper
    {
        public  string APP_DATA_PATH = "";

        public  void DoDeploy(Client client, string deployScript, string dataSQL)
        {
            var service = new ClientService();

            string logs = client.DeploymentLogs;

            try
            {
                AzureSQLHelper.CreateDB(client.DBName, client.DBServer,  System.Web.Hosting.HostingEnvironment.MapPath(AppSettings.CertificatePath),
                    AppSettings.CertificatePassword, true);
            }
            catch (Exception ex)
            {
                client.Status = ClientStatus.Error.ToString();
                logs += string.Format("<br/> <strong>{0:MM/dd/yy H:mm:ss zzz}</strong>  {1}", DateTime.Now, ex.Message);
                client.DeploymentLogs = logs;
                service.Update(client);
            }


            //Thread.Sleep(60 * 1000);
            AzureSQLHelper.CreateLoginUser(client.DBName, client.DBServer, client.DBUser, client.DBPassword,
                System.Web.Hosting.HostingEnvironment.MapPath(AppSettings.CertificatePath), AppSettings.CertificatePassword);
            /// Initial database structure
            AzureSQLHelper.ExecuteSQL(client.DBName, client.DBServer, deployScript);
            ///Call azure helper to activate services
            ///

            AzureSQLHelper.ExecuteSQL(client.DBName, client.DBServer, dataSQL);
        }


        public  async void CreateTenantAdmin(Client client, EIPUser eipUser)
        {
            EIPUserService eipService = new EIPUserService();
            var db = ApplicationDbContext.Create(AzureSQLHelper.BuildConnectionString(client.DBName, client.DBServer, client.DBUser, client.DBPassword));
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var user = new ApplicationUser { UserName = eipUser.Email, Email = eipUser.Email, FirstName = eipUser.Firstname, LastName = eipUser.Lastname, Status = UserStatus.TemporaryPassword.ToString() };
            string password = PasswordGenerator.Generate(12);
            var result = await manager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var roleresult = manager.AddToRole(user.Id, RoleName.CompanyAdmin);
                var rootUrl = AppSettings.EIPWebUrl;

                UserMailer mailer = new UserMailer();
                client.Status = ClientStatus.Deployed.ToString();

                var clientService = new ClientService();
                clientService.Update(client);
                SendWelcomeEmail(eipUser, rootUrl, password);

                /*
                mailer.Welcome(user.Email, new WelcomeMailViewModel()
                {
                    Name = user.FirstName,
                    Password = password,
                    Username = user.Email,
                    Url = rootUrl
                }).Send();
                 */
            }
        }

        public  void SendWelcomeEmail(EIPUser user, string siteUrl, string password)
        {
            // Prepare Postal classes to work outside of ASP.NET request
            var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/TrialUserMailer/"));
            var engines = new ViewEngineCollection();
            engines.Add(new FileSystemRazorViewEngine(viewsPath));

            var emailService = new Postal.EmailService(engines);

            // Get comment and send a notification.

            var email = new WelcomeTrialUserEmail
                {
                    To = user.Email,
                    UserName = user.Email,
                    SiteURL = siteUrl,
                    Firstname = user.Firstname,
                    Password = password
                    
                };

                emailService.Send(email);
            
        }
        public void Debug(string subject, string body)
        {
            string to = "debugme@mailinator.com";
            string from = "admin@kumo-eip.com";
            var message = new System.Net.Mail.MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;
            var client = new System.Net.Mail.SmtpClient("mail.kumo-eip.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.Credentials = new NetworkCredential("admin@kumo-eip.com", "!Admin20150420");

            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.
            //client.UseDefaultCredentials = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                      ex.ToString());
                //throw ex;
            }
        }
 
             
        public  void CreateTrialDB()
        {
            var service = new EIPUserService();
            var trialUser = service.GetAll().OrderByDescending(p => p.Id).FirstOrDefault(p => p.ClientId == null && p.Type == EIPUserTypes.Trial.ToString());


            if (trialUser != null)
            {
                ClientService clientSvc = new ClientService();
                var components = new ComponentService().GetAll().ToList().Select(p => p.Id.ToString());
                string allcom = components.Aggregate((current, next) => current + "," + next);

                Client client = new Client()
                {
                    DBName = "admin-" + trialUser.Company.Replace(" ", "") + "-trial",
                    ClientName = trialUser.Company + " (trial)",
                    DBPassword = PasswordGenerator.Generate(12),
                    DBUser = trialUser.Email.Replace("@", "_").Replace("-", "_").Replace(".", ""),
                    DBServer = "e4lfwb7eb4.database.windows.net",
                    SiteUrl = "www ",
                    Status = ClientStatus.TrialRegistered.ToString(),
                    ActiveComponents = allcom,
                   

                };
                clientSvc.Add(client);

                trialUser.ClientId = client.Id;
                service.Update(trialUser);

                var schemaSQL = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data/Deployment.sql"));

                var dataSQL = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data/InitialData.sql"));

                //Debug("Debug CreateTrialDB", "DoDeploy");
                DoDeploy(client, schemaSQL, dataSQL);
                //Debug("Debug CreateTrialDB", "End DoDeploy");
                client.Status = ClientStatus.PendingTrialUser.ToString();
                client.DeployedDate = DateTime.Now;

                clientSvc.Update(client);


                CreateTenantAdmin(client, trialUser);
                //create client data

            }
        }
    }
}