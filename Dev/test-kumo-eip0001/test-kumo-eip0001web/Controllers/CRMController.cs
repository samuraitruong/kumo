using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
using test_kumo_eip0001web.Models;
using PagedList;
using System.Linq.Dynamic;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Utility;
using test_kumo_eip0001model.Resources;


namespace test_kumo_eip0001web.Controllers
{
    [Authorize]
    [RedirectingAction(Action="CRM")]
    public class CRMController : KumoBaseController
    {
        private CustomerService customerService = new CustomerService();
        private EmployeeService employeeService = new EmployeeService();
        private IndustryService industryService = new IndustryService();

        public CRMController()
        {
            ViewBag.LogoUrl = "/Content/images/CRMLogo.jpg";
            ViewBag.Url = "/crm";
        }

        // GET: Customers
        [Permission(Component = "CRM", Action = Actions.View)]
        public ActionResult Index(int page=1, int pagesize=20, string orderby="Id", string keyword="", string order="desc")
        {
            ViewBag.SortOrder = ( order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var customers = customerService.GetAll()
                .Include(x => x.Employee)
                .Include(x => x.Industry)
                .OrderBy(orderByExpression);

            if(!string.IsNullOrEmpty(keyword)) {
                customers = customers.Where(p => p.CompanyName.Contains(keyword) || 
                                                 p.CompanyEmailAddress.Contains(keyword)  ||
                                                 p.Industry.Name.Contains(keyword) ||
                                                 p.Phone.Contains(keyword));
            };

             var pageData = customers.ToPagedList(page, pagesize);
             return View(pageData);
        }

        // GET: Customers/Details/5
        [Permission(Component = "CRM", Action = Actions.View)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customerService.GetAll()
                .Include(x => x.Employee)
                .Include(x => x.Industry)
                .Where(x => x.Id == id.Value)
                .SingleOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Permission(Component = "CRM", Action = Actions.Add)]
        public ActionResult Add()
        {
            ViewBag.AssignedTo = new SelectList(employeeService.GetAll().ToList(), "Id", "Name");
            ViewBag.IndustryId = new SelectList(industryService.GetAll().ToList(), "Id", "Name");
            ViewBag.LeadTypes = customerService.GetLeadTypes();
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "CRM", Action = Actions.Add)]
        public ActionResult Add([Bind(Include = "Id,Website,FaxNumber,CompanyAddress,Phone,CompanyName,CompanyEmailAddress,Source,LeadType,CustomerId,AssignedTo,IndustryId,Description")] CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                Customer cust = new Customer();
                cust.CopyFrom<Customer>(customer);
                cust.CustomerId = "C" + DateTime.Now.ConvertToZoneTime().ToString(StringResources.ComponentIdFormat);
                customerService.Add(cust);
                return RedirectToAction("Index");
            }

            ViewBag.AssignedTo = new SelectList(employeeService.GetAll().ToList(), "Id", "Name", customer.AssignedTo);
            ViewBag.IndustryId = new SelectList(industryService.GetAll().ToList(), "Id", "Name", customer.IndustryId);
            ViewBag.LeadTypes = customerService.GetLeadTypes();
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Permission(Component = "CRM", Action = Actions.Edit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customerService.GetAll()
                .Include(x => x.Employee)
                .Include(x => x.Industry)
                .Where(x => x.Id == id.Value)
                .SingleOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }
            var vm = new CustomerViewModel();
            vm.CopyFrom<CustomerViewModel>(customer);

            ViewBag.AssignedTo = new SelectList(employeeService.GetAll().ToList(), "Id", "Name", customer.AssignedTo);
            ViewBag.IndustryId = new SelectList(industryService.GetAll().ToList(), "Id", "Name", customer.IndustryId);
            ViewBag.LeadTypes = customerService.GetLeadTypes();
            return View(vm);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "CRM", Action = Actions.Edit)]
        public ActionResult Edit([Bind(Include = "Id,Website,FaxNumber,CompanyAddress,Phone,CompanyName,CompanyEmailAddress,Source,LeadType,CustomerId,AssignedTo,IndustryId,Description")] CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                var proxyCustomer = new Customer();
                proxyCustomer.CopyFrom<Customer>(customer);

                customerService.Update(proxyCustomer);
                return RedirectToAction("Index");
            }
            ViewBag.AssignedTo = new SelectList(employeeService.GetAll().ToList(), "Id", "Name", customer.AssignedTo);
            ViewBag.IndustryId = new SelectList(industryService.GetAll().ToList(), "Id", "Name", customer.IndustryId);
            ViewBag.LeadTypes = customerService.GetLeadTypes();
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Permission(Component = "CRM", Action = Actions.Delete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customerService.GetAll()
                .Include(x => x.Employee)
                .Include(x => x.Industry)
                .Where(x => x.Id == id.Value)
                .SingleOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [Permission(Component = "CRM", Action = Actions.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = customerService.GetById(id);
            customerService.Delete(customer);

            return RedirectToAction("Index");
        }
    }
}
