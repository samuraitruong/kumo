using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using PagedList;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
using test_kumo_eip0001repositories.DataModel;
using test_kumo_eip0001model.ViewModels;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]
    [RedirectingAction(Action = "CCS")]
    public class CCSController : Controller
    {
        private AnnouncementService service = new AnnouncementService();

        public CCSController()
        {
            ViewBag.LogoUrl = "/Content/images/CCSLogo.jpg";
            ViewBag.Url = "/ccs";
        }

        // GET: CCS
        [Permission(Component = "CCS", Action = Actions.View)]
        public ActionResult Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {


            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var announcements = service.GetAll()
                .OrderBy(orderByExpression);

            if (!string.IsNullOrEmpty(keyword))
            {
                announcements = announcements.Where(p => p.Title.Contains(keyword));
            };

            var pageData = announcements.ToPagedList(page, pagesize);
            return View(pageData);
        }

        // GET: CCS/Details/5
        [Permission(Component = "CCS", Action = Actions.View)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = service.GetById(id.Value);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: CCS/Create
        [Permission(Component = "CCS", Action = Actions.Add)]
        public ActionResult Add()
        {
            return View();
        }

        // POST: CCS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "CCS", Action = Actions.Add)]
        public ActionResult Add([Bind(Include = "Id,Title,Body,Created,Modified,PublishedDate")] AnnouncementViewModel announcement)
        {
            if (ModelState.IsValid)
            {
                var dmObject = new Announcement();
                dmObject.CopyFrom<Announcement>(announcement);

                dmObject.Created = DateTime.Now.ConvertToZoneTime();
                dmObject.Modified = DateTime.Now.ConvertToZoneTime();
                dmObject.PublishedDate = DateTime.Now.ConvertToZoneTime();

                service.Add(dmObject);
                return RedirectToAction("Index");
            }

            return View(announcement);
        }

        // GET: CCS/Edit/5
        [Permission(Component = "CCS", Action = Actions.Edit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = service.GetById(id.Value);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            var annoucementModel = new AnnouncementViewModel();
            annoucementModel.CopyFrom<AnnouncementViewModel>(announcement);

            return View(annoucementModel);
        }

        // POST: CCS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Component = "CCS", Action = Actions.Edit)]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Created,Modified,PublishedDate")] AnnouncementViewModel announcement)
        {
            if (ModelState.IsValid)
            {
                var dmObject = new Announcement();
                dmObject.CopyFrom<Announcement>(announcement);

                service.Update(dmObject);
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        // GET: CCS/Delete/5
        [Permission(Component = "CCS", Action = Actions.Delete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = service.GetById(id.Value);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: CCS/Delete/5
        [Permission(Component = "CCS", Action = Actions.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = service.GetById(id);
            service.Delete(announcement);
           
            return RedirectToAction("Index");
        }
    }
}
