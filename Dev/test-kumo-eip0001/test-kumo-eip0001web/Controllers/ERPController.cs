using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
using PagedList;
using System.Linq.Dynamic;
using test_kumo_eip0001model.ViewModels;
using System.Collections.Generic;
using System.Web.Services;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]
    [RedirectingAction(Action = "ERP")]
    public class ERPController : KumoBaseController
    {
        private CalendarService service = new CalendarService();

        public ERPController()
        {
            ViewBag.LogoUrl = "/Content/images/ERPLogo.jpg";
            ViewBag.Url = "/erp/Calendar";
        }

        [HttpPost]
        [Permission(Component = "ERP", Action = Actions.View)]
        public ActionResult ERPCalendarFull()
        {
            IEnumerable<test_kumo_eip0001model.Calendar> calendarItems = service.GetAll().Select(p => new { Id = p.Id, JobScope = p.JobScope, StartDate = p.StartDate, DueDate = p.DueDate }).ToList()
                .Select(p => new test_kumo_eip0001model.Calendar() { Id = p.Id, JobScope = p.JobScope, StartDate = p.StartDate, DueDate = p.DueDate });
            return Json(calendarItems);
        }

        [Permission(Component = "ERP", Action = Actions.View)]
        public ActionResult Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {
            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var calendarItems = service.GetAll()
                .Include(p => p.User)
                .OrderBy(orderByExpression);

            if (!string.IsNullOrEmpty(keyword))
            {
                calendarItems = calendarItems.Where(p =>
                                                 p.JobScope.Contains(keyword) ||
                                                 p.User.Email.Contains(keyword) ||
                                                 p.User.Firstname.Contains(keyword) ||
                                                 p.User.Lastname.Contains(keyword) ||
                                                 p.User.UserName.Contains(keyword));
            };

            var pageData = calendarItems.ToPagedList(page, pagesize);
            return View(pageData);
        }

        [Permission(Component = "ERP", Action = Actions.View)]
        public ActionResult Calendar()
        {
            return View();
        }

        // GET: Calendar/Details/5
        [Permission(Component = "ERP", Action = Actions.View)]
        public ActionResult Details(int? id, string src)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar item = service.GetById(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.src = src;
            return View(item);
        }

        // GET: Calendar/Create
        [Permission(Component = "ERP", Action = Actions.Add)]
        public ActionResult Add(DateTime? startDate)
        {
            InitialForgeinData();
            var newModel = new CalendarViewModel();
            newModel.StartDate = DateTime.Today;
            newModel.DueDate = DateTime.Today;
            newModel.CompletedDate = DateTime.Today;
            if (startDate.HasValue) newModel.StartDate = startDate.Value;
            return View(newModel);
        }

        // POST: Calendar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "ERP", Action = Actions.Add)]
        public ActionResult Add([Bind(Include = "Id,JobScope,Priority,StartDate,DueDate,CompletedDate,AssignedTo,CompletedPercent,Status")] CalendarViewModel calendar)
        {
            if (ModelState.IsValid)
            {
                Calendar calendarItem = new Calendar();
                calendarItem.CopyFrom<Calendar>(calendar);

                service.Add(calendarItem);
                return RedirectToAction("Index");
            }

            InitialForgeinData(priority: calendar.Priority, status: calendar.Status, assigned: calendar.AssignedTo, percent: calendar.CompletedPercent);
            return View(calendar);
        }

        private void InitialForgeinData(string priority = "", string status = "", string assigned = "", decimal percent = 0)
        {
            ViewBag.Status = new SelectList(service.GetJobStatus(), "Name", "Id", status);

            ViewBag.Priority = new SelectList(service.GetPriorityStatus(), "Name", "Id", priority);

            var users = (new ServiceBase<AspNetUser>()).GetAll().ToList();

            var proxyObjects = from p in users
                               select new { Id = p.Id, Fullname = p.Firstname + " " + p.Lastname };

            ViewBag.AssignedTo = new SelectList(proxyObjects, "Id", "FullName", assigned);

            ViewBag.CompletedPercent = new SelectList(new[] {
                            new {Name="0%", Id="0"},
                            new {Name="25%", Id="0.25"},
                            new {Name="50%", Id="0.50"},
                            new {Name="75%", Id="0.75"},
                            new {Name="100%", Id="1.00"}
                            }, "Id", "Name", percent);
        }

        // GET: Calendar/Edit/5
        [Permission(Component = "ERP", Action = Actions.Edit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendarItem = service.GetById(id.Value);
            if (calendarItem == null)
            {
                return HttpNotFound();
            }
            var carModel = new CalendarViewModel();
            carModel.CopyFrom<CalendarViewModel>(calendarItem);

            InitialForgeinData(priority: calendarItem.Priority, status: calendarItem.Status, assigned: calendarItem.AssignedTo, percent: calendarItem.CompletedPercent ?? 0);

            return View(carModel);
        }

        // POST: Calendar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "ERP", Action = Actions.Edit)]
        public ActionResult Edit([Bind(Include = "Id,JobScope,Priority,StartDate,DueDate,CompletedDate,AssignedTo,CompletedPercent,Status")] CalendarViewModel calendar)
        {
            if (ModelState.IsValid)
            {
                var calItem = new Calendar();
                calItem.CopyFrom<Calendar>(calendar);

                service.Update(calItem);
                return RedirectToAction("Index");
            }

            InitialForgeinData(priority: calendar.Priority, status: calendar.Status, assigned: calendar.AssignedTo, percent: calendar.CompletedPercent);
            return View(calendar);
        }

        // GET: Calendar/Delete/5
        [Permission(Component = "ERP", Action = Actions.Delete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Calendar item = service.GetAll().Where(x => x.Id == id.Value).SingleOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Calendar/Delete/5
        [Permission(Component = "ERP", Action = Actions.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Calendar item = service.GetById(id);
            service.Delete(item);

            return RedirectToAction("Index");
        }
    }
}
