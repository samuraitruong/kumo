using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Security;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Linq.Dynamic;
using System.Net;
using KUMO.CentralAdmin.Web.Mailers;
using KUMO.CentralAdmin.Web;
using KUMO.CentralAdmin.Web.Models;
using KUMO.CentralAdmin.Application;
using KUMO.CentralAdmin.Model;
using KUMO.CentralAdmin.Model.ViewModels;
using Hangfire;
using System.Threading;

using KUMO.CentralAdmin.Repositories.DataModel;
using Microsoft.AspNet.Identity.EntityFramework;
using KUMO.CentralAdmin.Web.Utilities;
using System.Data.Entity;

namespace KUMO.CentralAdmin.Web.Controllers
{
    [Authorize]
    [ValidateInput(false)]
    [AdminOnly]
    public class ClientsController : Controller
    {
        private Entities db = new Entities();
        private ClientService service;
        private ComponentService componentService;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private UserService userService;

        public ClientsController()
        {
            service = new ClientService();
            componentService = new ComponentService();
        }
        public ClientsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            service = new ClientService();
            componentService = new ComponentService();

            UserManager = userManager;
            SignInManager = signInManager;
            userService = new UserService();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {

                _userManager = value;
            }
        }

        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }


        
        // GET: Clients
        public async Task<ActionResult> Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {
            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            
            var components = componentService.GetAll().ToList();
            var clients = service.GetAll().OrderBy(orderByExpression);
            if (!string.IsNullOrEmpty(keyword))
            {
                clients = clients.Where(p => p.ClientName.Contains(keyword));
            };

            clients.ToList().ForEach(client =>
            {
                if (!string.IsNullOrEmpty(client.ActiveComponents))
                {
                    var parts = client.ActiveComponents.Split(new char[] { ',' }).ToList();
                    var selectedItems = components.Where(i => parts.Any(x => x == i.Id.ToString()));

                    client.ComponentNames = string.Join(", ", selectedItems.Select(x => x.Name));
                }
            });


            var pageData = clients.ToPagedList(page, pagesize);
            return View(pageData);
        }

        // GET: Clients/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await db.Clients.FindAsync(id);
            var components = componentService.GetAll().ToList();
            if (!string.IsNullOrEmpty(client.ActiveComponents))
            {
                var parts = client.ActiveComponents.Split(new char[] { ',' }).ToList();
                var selectedItems = components.Where(i => parts.Any(x => x == i.Id.ToString()));

                client.ComponentNames = string.Join(", ", selectedItems.Select(x => x.Name));
            }

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        [HttpGet]
        public ActionResult Deploy(int id, int time=0)
        {
            //display deploying services

            ///Call azure helper to activate services
            return View();
        }

        public void DoDeploy(Client client, string deployScript, string dataSQL)
        {
            string logs = client.DeploymentLogs;

            try
            {
                AzureSQLHelper.CreateDB(client.DBName, client.DBServer, Server.MapPath(AppSettings.CertificatePath), 
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
                Server.MapPath(AppSettings.CertificatePath), AppSettings.CertificatePassword);
            /// Initial database structure
            AzureSQLHelper.ExecuteSQL(client.DBName, client.DBServer, deployScript);
            ///Call azure helper to activate services
            ///

            AzureSQLHelper.ExecuteSQL(client.DBName, client.DBServer, dataSQL);
        }
        [HttpPost]
        public ActionResult Deploy(int id)
        {
            //TODO : Move these code to Application Layer
            var client = db.Clients.FirstOrDefault(p => p.Id == id);
            Thread.Sleep(2000);
            
            if(client == null && client.Status != ClientStatus.Pending.ToString()) {
                return Content("false");
            }
            var sqlText = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/Deployment.sql"));

            var dataSQL = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/InitialData.sql"));
           

            client.Status = ClientStatus.Deploying.ToString();

            db.SaveChanges();
           //var jobId = BackgroundJob.Enqueue(() => DoDeploy(client, sqlText));

            DoDeploy(client, sqlText, dataSQL);

            //client.Status = ClientStatus.Deployed.ToString();

            //db.SaveChanges();

            return Content("true");
        }
        // GET: Clients/Create
        public ActionResult Add()
        {
            Client client = new Client()
            {
                DBPassword = PasswordGenerator.Generate(12),
                DBServer = AppSettings.DBServer,
                Status = ClientStatus.Pending.ToString(),
                SiteUrl = AppSettings.EIPWebUrl,
                Components = componentService.GetAll().ToList()
            };
            var listserver = AzureSQLHelper.GetListServers(Server.MapPath(AppSettings.CertificatePath), AppSettings.CertificatePassword);

            var select = listserver.Select(p => new { Name = p, id = p }).ToList();

            ViewBag.DBServer =new SelectList(select, "Name", "Id");
            
            return View(client);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "Id,ClientName,DBName,DBUser,DBPassword,DBServer,Status,SiteUrl,SelectedComponents")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (client.SelectedComponents != null)
                {
                    client.ActiveComponents = string.Join(",", client.SelectedComponents);
                }
                client.DeploymentLogs = "";
                db.Clients.Add(client);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var listserver = AzureSQLHelper.GetListServers(Server.MapPath(AppSettings.CertificatePath), AppSettings.CertificatePassword);

            var select = listserver.Select(p => new { Name = p, id = p }).ToList();

            ViewBag.DBServer = new SelectList(select, "Name", "Id");

            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ComponentId = new SelectList(componentService.GetAll().ToList(), "Id", "Name");
            Client client = await db.Clients.FindAsync(id);
            if (!string.IsNullOrEmpty(client.ActiveComponents))
            {
                var ids = client.ActiveComponents.Split(new char[] { ',' }).ToList();

                List<int> componentIds = new List<int>();
                ids.ForEach(i => { componentIds.Add(int.Parse(i)); });
                client.SelectedComponents = componentIds.ToArray();
            }

            client.Components = componentService.GetAll().ToList();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClientName,DBName,DBUser,DBPassword,DBServer,Status,SiteUrl,SelectedComponents")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (client.SelectedComponents != null)
                {
                    client.ActiveComponents = string.Join(",", client.SelectedComponents);
                }
                db.Entry(client).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await db.Clients.FindAsync(id);
            var components = componentService.GetAll().ToList();
            if (!string.IsNullOrEmpty(client.ActiveComponents))
            {
                var parts = client.ActiveComponents.Split(new char[] { ',' }).ToList();
                var selectedItems = components.Where(i => parts.Any(x => x == i.Id.ToString()));

                client.ComponentNames = string.Join(", ", selectedItems.Select(x => x.Name));
            }

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Client client = await db.Clients.FindAsync(id);
            AzureSQLHelper.DeleteDB(client.DBName, client.DBServer, Server.MapPath(AppSettings.CertificatePath), AppSettings.CertificatePassword);
            db.Clients.Remove(client);
            await db.SaveChangesAsync();
           
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
