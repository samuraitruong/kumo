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
using test_kumo_eip0001web.Models;
using test_kumo_eip0001application;
using System.Web.Security;
using test_kumo_eip0001model;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001web.Mailers;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Utility;
using PagedList;
using System.Linq.Dynamic;
using System.Net;
using System.Configuration;
using RestSharp;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]
    [RedirectingAction(Action = "HR")]
    public class HRController : KumoBaseController
    {
        private EmployeeService service = new EmployeeService();

        public HRController()
        {
            ViewBag.LogoUrl = "/Content/images/HRLogo.jpg";
            ViewBag.Url = "/hr";
        }

        [Permission(Component = "HR", Action = Actions.View)]
        public ActionResult Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {


            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var employees = service.GetAll()
                .Include(p=>p.Manager)
                .OrderBy(orderByExpression);

            if (!string.IsNullOrEmpty(keyword))
            {
                employees = employees.Where(p => p.Firstname.Contains(keyword) ||
                                                 p.Lastname.Contains(keyword) ||
                                                 p.Fullname.Contains(keyword) ||                                                
                                                 p.Email.Contains(keyword) ||
                                                 p.Address.Contains(keyword));
                                                  
                                                 //p.JobTitle.Contains(keyword));
            };

            var pageData = employees.ToPagedList(page, pagesize);
            return View(pageData);
        }

        
        // GET: Employees/Details/5
        [Permission(Component = "HR", Action = Actions.View)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = service.GetById(id.Value);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        [Permission(Component = "HR", Action = Actions.Add)]
        public ActionResult Add()
        {
            ViewBag.LineManager = new SelectList(service.GetAll().ToList(), "Id", "FullName");
            InitialForgeinData();
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "HR", Action = Actions.Add)]
        //public ActionResult Add([Bind(Include = "DateOfBirth, EffectiveDate,DepartmentID, JobTitle, Status,Id,Firstname,Lastname,Address,Phone,Email, DirectPhone,Fullname, LineManager")] EmployeeViewModel employee)
        public async Task<ActionResult> Add(Employee employee)
        {
            if (ModelState.IsValid)
            {

                if (TenantHelper.IsExistingUser(employee.Email))
                {
                    ModelState.AddModelError("", "Email already exist");
                    return View(employee);
                }

                 var db = ApplicationDbContext.Create(SessionManager.PrivateTenantConnectionString);

                var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

                var signMgr = new ApplicationSignInManager(UserManager, HttpContext.GetOwinContext().Authentication);

                string password = PasswordGenerator.Generate(12);

                var user = new ApplicationUser { UserName = employee.Email, Email = employee.Email, FirstName = employee.Firstname, LastName = employee.Lastname, Status = UserStatuses.TemporaryPassword.ToString() };
                var result =  await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    TenantHelper.AddEIPUser(employee.Email, employee.Firstname, employee.Lastname, RoleNames.User.ToString());
                    var roleresult = UserManager.AddToRole(user.Id, RoleNames.User);

                    var rootUrl = Url.Action("Index", "Home", routeValues: null, protocol: Request.Url.Scheme);

                    var userMailer = new UserMailer();
                    //add code send email from here.
                    userMailer.Welcome(user.Email, new WelcomeMailViewModel()
                    {
                        Name = user.FirstName,
                        Password = password,
                        Username = user.Email,
                        Url = rootUrl
                    }).Send();

                    //var emp = new Employee();
                    //emp.CopyFrom<Employee>(employee);
                    employee.LineManagerId = employee.LineManagerId == 0 ? null : employee.LineManagerId;

                    employee.EmployeeId = "E" + DateTime.Now.ConvertToZoneTime().ToString(StringResources.ComponentIdFormat);
                    //employee.LineManagerId = null;
                    employee.UserId = user.Id;

                    service.Add(employee);
                    if (IsDlg)
                    {
                        return ClosePopup();
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            //ViewBag.LineManager = new SelectList(service.GetAll().ToList(), "Id", "FullName");
            //InitialForgeinData(department: employee.DepartmentID, status: employee.Status);
       
            return View(employee);
        }

        private void InitialForgeinData(int department = -1,  string status = "")
        {
           

            ViewBag.Status = new SelectList(new[] { 
               new  {Name= StringResources.EmployeeStatus_Current, Id=StringResources.EmployeeStatus_Current},
                new  {Name=StringResources.EmployeeStatus_Fired, Id=StringResources.EmployeeStatus_Fired},
                new  {Name=StringResources.EmployeeStatus_Resigned, Id=StringResources.EmployeeStatus_Resigned}
            }, "Name", "Id", status);


           
            ViewBag.DepartmentID = new SelectList((new DepartmentService()).GetAll().ToList(), "Id", "Name", department);

           
          
        }

        // GET: Employees/Edit/5
        [Permission(Component = "HR", Action = Actions.Edit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = service.GetById(id.Value);                  
            if (employee == null)
            {
                return HttpNotFound();
            }
            var empModel = new EmployeeViewModel();
            empModel.CopyFrom<EmployeeViewModel>(employee);

            ViewBag.LineManager = new SelectList(service.GetAll().Where(p => p.Id != employee.Id).ToList(), "Id", "FullName", employee.LineManagerId);
            InitialForgeinData(department: employee.DepartmentID, status: employee.EmploymentStatusId.ToString());
           
            return View(empModel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "HR", Action = Actions.Edit)]
        public ActionResult Edit([Bind(Include = "DateOfBirth, EffectiveDate,DepartmentID, JobTitle, Status,Id,Firstname,Lastname,Address,Phone,Email, DirectPhone,Fullname, EmployeeId, LineManager")] EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employee();
                emp.CopyFrom<Employee>(employee);
                emp.LineManagerId = emp.LineManagerId == 0 ? null : emp.LineManagerId;

                service.Update(emp);
                return RedirectToAction("Index");
            }

            ViewBag.LineManager = new SelectList(service.GetAll().Where(p => p.Id != employee.Id).ToList(), "Id", "FullName", employee.LineManager);
            InitialForgeinData( department: employee.DepartmentID,  status: employee.Status);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Permission(Component = "HR", Action = Actions.Delete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = service.GetAll().Include(x => x.Customers).Where(x => x.Id == id.Value).SingleOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permission(Component = "HR", Action = Actions.Delete)]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = service.GetAll().Include(x => x.Customers).Where(x => x.Id == id).SingleOrDefault();
            if (employee.Customers.Count > 0 || employee.Employees.Count > 0)
            {
                return View(employee);
            }
            service.Delete(employee);
            return RedirectToAction("Index");
        }
    }
}
