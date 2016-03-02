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

namespace KUMO.CentralAdmin.Web.Controllers
{
    [Authorize]
    [AdminOnly]
    public class EIPUsersController : BaseController
    {
        private ClientService service;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IUserMailer _userMailer = new UserMailer();
        private EIPUserService eipService;
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        public EIPUsersController()
        {
            service = new ClientService();
            eipService = new EIPUserService();
        }

        public EIPUsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            service = new ClientService();
            UserManager = userManager;
            SignInManager = signInManager;
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

        // GET: EIPUsers
        public ActionResult Index(int id)
        {
            ViewData[CAConstants.EXTRA_ROUTE_DATA] = new { id = id };
            
            ViewBag.Client = (new ClientService()).GetById(id);

            return View(eipService.GetAll().Where(i => i.ClientId == id));
        }

        [HttpGet]
        public ActionResult Add(int id)
        {
            EIPUser model = new EIPUser()
            {
                ClientId = id
            };

            ViewData[CAConstants.EXTRA_ROUTE_DATA] = new { id = id };
            ViewBag.Client = (new ClientService()).GetById(id);
            return View(model);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(EIPUser model)
        {
            var password = PasswordGenerator.Generate();

            if (ModelState.IsValid)
            {
                EIPUserService eipService = new EIPUserService();
                var eipUser = eipService.GetAll().FirstOrDefault(p => p.Email == model.Email);
                if (eipUser != null)
                {
                    ModelState.AddModelError("", "Email already existing in database.");
                    return View(model);
                }
                else
                {
                    model.IsAdmin = true;
                    eipService.Add(model);

                    var client = service.GetById(model.ClientId.Value);
                    var db = ApplicationDbContext.Create(AzureSQLHelper.BuildConnectionString(client.DBName, client.DBServer, client.DBUser, client.DBPassword));
                    var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.Firstname, LastName = model.Lastname, Status = UserStatus.TemporaryPassword.ToString() };
                    var result = await manager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        var roleresult = manager.AddToRole(user.Id, RoleName.CompanyAdmin);
                        var rootUrl = AppSettings.EIPWebUrl;

                        UserMailer.Welcome(user.Email, new WelcomeMailViewModel()
                        {
                            Name = user.FirstName,
                            Password = password,
                            Username = user.Email,
                            Url = rootUrl
                        }).Send();

                        return RedirectToAction("Index", "EIPUsers", new { id = model.ClientId });
                    }
                    //AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AdminOnly]
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EIPUser user = eipService.GetById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewData[CAConstants.EXTRA_ROUTE_DATA] = new { id = user.ClientId.Value };
            return View(user);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,ClientId,Firstname,Lastname")] EIPUser userModel)
        {
            if (ModelState.IsValid)
            {
                eipService.Update(userModel);
                return RedirectToAction("Index", new { @id = userModel.ClientId.Value });
            }
            return View(userModel);
        }

        [AdminOnly]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = eipService.GetById(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewData[CAConstants.EXTRA_ROUTE_DATA] = new { id = user.ClientId.Value };
            return View(user);
        }

        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = eipService.GetById(id);
            var clientId = user.ClientId;
            eipService.Delete(user);

            return RedirectToAction("Index", new { @id = clientId });
        }
    }
}