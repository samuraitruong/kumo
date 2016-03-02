using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test_kumo_eip0001model;
using PagedList;
using test_kumo_eip0001application;
using test_kumo_eip0001model.ViewModels;
using System.Linq.Dynamic;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]

    public class IssueTrackerController : KumoBaseController
    {
        private ApplicationUserManager _userManager;
        private IssueTrackerService service = new IssueTrackerService();
        public IssueTrackerController()
        {

        }
        public IssueTrackerController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        // GET: IssueTracker
        public ActionResult Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {
            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var issues = service.GetAll()
                                .OrderBy(orderByExpression);
            if (!string.IsNullOrEmpty(keyword))
            {
                issues = issues.Where(p => p.Name.Contains(keyword) ||
                                                 p.Description.Contains(keyword) ||
                                                 p.Resolution.Contains(keyword));
            };

            var pageData = issues.ToPagedList(page, pagesize);
            return View(pageData);
        }

        // GET: IssueTracker/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueTracker issueTracker = service.GetById(id.Value);
            if (issueTracker == null)
            {
                return HttpNotFound();
            }
            return View(issueTracker);
        }

        // GET: IssueTracker/Create
        public ActionResult Add()
        {
            InitialForgeinData();

            return View();
        }

        private void InitialForgeinData(string priority="", string category="", string status="", string assigned="")
        {
            ViewBag.AssignedTo = new SelectList((new EmployeeService()).GetAll().ToList(), "Id", "FullName");
            ViewBag.Priority = new SelectList(new[] { 
                new  {Name= IssueTrackerResource.Priority_High, Id= IssueTrackerResource.Priority_High},
                new  {Name=IssueTrackerResource.Priority_Medium, Id=IssueTrackerResource.Priority_Medium},
                new  {Name=IssueTrackerResource.Priority_Low, Id=IssueTrackerResource.Priority_Low}
            }, "Name", "Id", priority);

            ViewBag.Category = new SelectList(new[] { 
                new  {Name=IssueTrackerResource.Category_Time, Id=IssueTrackerResource.Category_Time},
                new  {Name=IssueTrackerResource.Category_Cost, Id=IssueTrackerResource.Category_Cost},
                new  {Name=IssueTrackerResource.Category_Quality, Id=IssueTrackerResource.Category_Quality},
                new  {Name=IssueTrackerResource.Category_Resource, Id=IssueTrackerResource.Category_Resource},
                 new  {Name=IssueTrackerResource.Category_Other, Id=IssueTrackerResource.Category_Other}
            }, "Name", "Id", category);

            ViewBag.Status = new SelectList(new[] { 
                new  {Name=IssueTrackerResource.Status_Active, Id=IssueTrackerResource.Status_Active},
                new  {Name=IssueTrackerResource.Status_Resolved, Id=IssueTrackerResource.Status_Resolved},
                new  {Name=IssueTrackerResource.Status_Closed, Id=IssueTrackerResource.Status_Closed}
            }, "Name", "Id", status);

            var users = (new ServiceBase<AspNetUser>()).GetAll().ToList();

            var proxyObjects = from p in users
                    select new { Id = p.Id, Fullname = p.Firstname + " " + p.Lastname };



            ViewBag.AssignedTo = new SelectList(proxyObjects, "Id", "FullName", assigned);

        }

        // POST: IssueTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Id,Name,Priority,Category,StartDate,DueDate,ClosedDate,AssignedTo,Status,Description,Resolution")] IssueTrackerViewModel issueTracker)
        {
            if (ModelState.IsValid)
            {
                IssueTracker issue = new IssueTracker();
                issue.CopyFrom<IssueTracker>(issueTracker);

                service.Add(issue);
                return RedirectToAction("Index");
            }

            InitialForgeinData(priority: issueTracker.Priority, category:issueTracker.Category, status:issueTracker.Status, assigned:issueTracker.AssignedTo);
            return View(issueTracker);
        }

        // GET: IssueTracker/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueTracker issueTracker = service.GetById(id.Value);
            if (issueTracker == null)
            {
                return HttpNotFound();
            }

            var issueModel = new IssueTrackerViewModel();
            issueModel.CopyFrom<IssueTrackerViewModel>(issueTracker);

            InitialForgeinData(priority: issueTracker.Priority, category:issueTracker.Category, status:issueTracker.Status, assigned:issueTracker.AssignedTo);
            return View(issueModel);
        }

        // POST: IssueTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Priority,Category,StartDate,DueDate,ClosedDate,AssignedTo,Status,Description,Resolution")] IssueTrackerViewModel issueTracker)
        {
            if (ModelState.IsValid)
            {
                var issue = new IssueTracker();
                issue.CopyFrom<IssueTracker>(issueTracker);
                

                service.Update(issue);
                return RedirectToAction("Index");
            }


            InitialForgeinData(priority: issueTracker.Priority, category: issueTracker.Category, status: issueTracker.Status);
           
            return View(issueTracker);
        }

        // GET: IssueTracker/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueTracker issueTracker = service.GetById(id.Value);
            if (issueTracker == null)
            {
                return HttpNotFound();
            }
            return View(issueTracker);
        }

        // POST: IssueTracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IssueTracker issueTracker = service.GetById(id);
            service.Delete(issueTracker);
            
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
