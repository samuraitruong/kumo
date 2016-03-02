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
using test_kumo_eip0001repositories.DataModel;
using test_kumo_eip0001application;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]

    [AdminOnly]
    public class DMSController : KumoBaseController
    {
        private DocumentLibraryService service;
        
        public DMSController() {
            service = new DocumentLibraryService();
            ViewBag.LogoUrl = "/Content/images/MISLogo.jpg";
            ViewBag.Url = "/mis";
        }
        // GET: DocumentLibraries
        public  ActionResult Index()
        {
            return View(service.GetAll().ToList());
        }

        // GET: DocumentLibraries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentLibrary documentLibrary = service.GetById(id.Value);
            if (documentLibrary == null)
            {
                return HttpNotFound();
            }
            return View(documentLibrary);
        }

        // GET: DocumentLibraries/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: DocumentLibraries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Id,Name,Description")] DocumentLibrary documentLibrary)
        {
            if (ModelState.IsValid)
            {
                documentLibrary.BlobContainer = CreateBlobContainerName(documentLibrary.Name);
                service.Add(documentLibrary);
                
                return RedirectToAction("Index");
            }

            return View(documentLibrary);
        }

        private string CreateBlobContainerName(string name)
        {

            string blob = SessionManager.CurrentTenant.Name + "-" + name;

            return blob.ToLower().Replace(" ", "-");


        }

        // GET: DocumentLibraries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentLibrary documentLibrary = service.GetById(id.Value);
            if (documentLibrary == null)
            {
                return HttpNotFound();
            }
            return View(documentLibrary);
        }

        // POST: DocumentLibraries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] DocumentLibrary documentLibrary)
        {
            if (ModelState.IsValid)
            {
                service.Update(documentLibrary);
                return RedirectToAction("Index");
            }
            return View(documentLibrary);
        }

        // GET: DocumentLibraries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentLibrary documentLibrary = service.GetById(id.Value);
            if (documentLibrary == null)
            {
                return HttpNotFound();
            }
            return View(documentLibrary);
        }

        // POST: DocumentLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentLibrary documentLibrary = service.GetById(id);
            service.Delete(documentLibrary);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               
            }
            base.Dispose(disposing);
        }
    }
}
