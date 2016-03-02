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

    public class TaskTrackerController : KumoBaseController
    {
        private ApplicationUserManager _userManager;
        private TaskTrackerService service = new TaskTrackerService();
        public TaskTrackerController()
        {

        }
        public TaskTrackerController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        // GET: TaskTracker
        public ActionResult Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {
            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var tasks = service.GetAll()
                                .OrderBy(orderByExpression);
            if (!string.IsNullOrEmpty(keyword))
            {
                tasks = tasks.Where(p => p.TaskName.Contains(keyword) ||
                                                 p.Description.Contains(keyword)
                                                );
            };

            var pageData = tasks.ToPagedList(page, pagesize);
            return View(pageData);
        }

        // GET: TaskTracker/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskTracker taskTracker = service.GetById(id.Value);
            if (taskTracker == null)
            {
                return HttpNotFound();
            }
            return View(taskTracker);
        }

        // GET: TaskTracker/Create
        public ActionResult Add()
        {
            InitialForgeinData();

            return View();
        }

        private void InitialForgeinData(string priority="", string status="", string assigned="", decimal percent=0)
        {
            ViewBag.AssignedTo = new SelectList((new EmployeeService()).GetAll().ToList(), "Id", "FullName");

            ViewBag.Priority = new SelectList(new[] { 
                new  {Name= TaskTrackerResource.Priority_High, Id= TaskTrackerResource.Priority_High},
                new  {Name=TaskTrackerResource.Priority_Medium, Id=TaskTrackerResource.Priority_Medium},
                new  {Name=TaskTrackerResource.Priority_Low, Id=TaskTrackerResource.Priority_Low}
            }, "Name", "Id", priority);

           

            ViewBag.Status = new SelectList(new[] { 
                new  {Name=TaskTrackerResource.Status_NotStarted, Id=TaskTrackerResource.Status_NotStarted},
                new  {Name=TaskTrackerResource.Status_InProgress, Id=TaskTrackerResource.Status_InProgress},
                new  {Name=TaskTrackerResource.Status_Completed, Id=TaskTrackerResource.Status_Completed},
                new  {Name=TaskTrackerResource.Status_Deferred, Id=TaskTrackerResource.Status_Deferred},
                new  {Name=TaskTrackerResource.Status_Late, Id=TaskTrackerResource.Status_Late}
            }, "Name", "Id", status);

             

            ViewBag.CompletedPercent = new SelectList(new[] {
                new {Name="0%", Id="0"},
          new {Name="10%", Id="0.1"}
          }, "Name","Id", percent);

        //new {Id=0, Text="0%"},
        //  new {Id=0.1, Text="10%"},
        //  new {Id=0.2, Text="20%"},
        //  new {Id=0.3, Text="30%"},
        //  new {Id=0.4, Text="40%"},
        //  new {Id=0.5, Text="50%"},
        //  new {Id=0.6, Text="60%"},
        //  new {Id=0.7, Text="70%"},
        //  new {Id=0.8, Text="80%"},
        //  new {Id=0.9, Text="90%"},
        //  new {Id=1, Text="100%"},
    //}, "Id", "Text", ViewData.TemplateInfo.FormattedModelValue, percent);

           

            var users = (new ServiceBase<AspNetUser>()).GetAll().ToList();

            var proxyObjects = from p in users
                    select new { Id = p.Id, Fullname = p.Firstname + " " + p.Lastname };



            ViewBag.AssignedTo = new SelectList(proxyObjects, "Id", "FullName", assigned);

        }

        // POST: TaskTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Id,TaskName,Priority,StartDate,DueDate,CompletedDate,AssignedTo,CompletedPercent,Status,Description")] TaskTrackerViewModel taskTracker)
        {
            if (ModelState.IsValid)
            {
                TaskTracker task = new TaskTracker();
                task.CopyFrom<TaskTracker>(taskTracker);

                service.Add(task);
                return RedirectToAction("Index");
            }

            InitialForgeinData(priority: taskTracker.Priority, status:taskTracker.Status, assigned:taskTracker.AssignedTo, percent:taskTracker.CompletedPercent);
            return View(taskTracker);
        }

        // GET: TaskTracker/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskTracker taskTracker = service.GetById(id.Value);
            if (taskTracker == null)
            {
                return HttpNotFound();
            }

            var taskModel = new TaskTrackerViewModel();
            taskModel.CopyFrom<TaskTrackerViewModel>(taskTracker);

            InitialForgeinData(priority: taskTracker.Priority, status: taskTracker.Status, assigned: taskTracker.AssignedTo, percent:taskTracker.CompletedPercent);
            return View(taskModel);
        }

        // POST: TaskTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TaskName,Priority,StartDate,DueDate,CompletedDate,AssignedTo,CompletedPercent,Status,Description")] TaskTrackerViewModel taskTracker)
        {
            if (ModelState.IsValid)
            {
                var task = new TaskTracker();
                task.CopyFrom<TaskTracker>(taskTracker);
                

                service.Update(task);
                return RedirectToAction("Index");
            }


            InitialForgeinData(priority: taskTracker.Priority, status: taskTracker.Status, assigned: taskTracker.AssignedTo, percent: taskTracker.CompletedPercent);
           
            return View(taskTracker);
        }

        // GET: TaskTracker/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskTracker taskTracker = service.GetById(id.Value);
            if (taskTracker == null)
            {
                return HttpNotFound();
            }
            return View(taskTracker);
        }

        // POST: TaskTracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaskTracker taskTracker = service.GetById(id);
            service.Delete(taskTracker);
            
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
