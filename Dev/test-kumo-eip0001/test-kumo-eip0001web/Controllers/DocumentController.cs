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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using MvcFileUploader.Models;
using MvcFileUploader;
using System.Collections.Generic;
using test_kumo_eip0001web.Utility;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO.Compression;

namespace test_kumo_eip0001web.Controllers
{
    [Authorize]
    public class DocumentController : KumoBaseController
    {
        private DocumentService service = new DocumentService();
        DocumentLibraryService libService;
        public DocumentController()
        {
            libService = new DocumentLibraryService();


            ViewBag.LogoUrl = "/Content/images/MISLogo.jpg";
            ViewBag.Url = "/mis";
        }

        public ActionResult Index(string doclib,int? root, int page = 1, int pagesize = 20, string orderby = "ItemType", string keyword = "", string order = "asc")
        {
            var library = libService.GetAll().FirstOrDefault(p => p.Name == doclib);
            if (library == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.UUID = library.Name;

            if (root.HasValue)
            {
                var currentFolder = service.GetById(root.Value);
                if (currentFolder == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ViewBag.FolderPaths = GetFolderPath<Document>(currentFolder.Id);
                ViewBag.UUID = currentFolder.Uuid;
            }

            ViewBag.RootFolder = root.HasValue?root.Value.ToString() : "";
            ViewBag.Library =  library;

            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var documents = service.GetAll()
                .Where(p => p.ParentId == root && p.DocumentLibraryId == library.Id)
               // .Include(p => p.AspNetUser)
                .OrderBy(orderByExpression);
            
            if (!string.IsNullOrEmpty(keyword))
            {
                documents = documents.Where(p => p.Name.Contains(keyword));
                                                /*||
                                                 p.AspNetUser\.Email.Contains(keyword) ||
                                                 p.User.Firstname.Contains(keyword) ||
                                                 p.User.Lastname.Contains(keyword) ||
                                                 p.User.UserName.Contains(keyword));*/
            };
            ViewBag.FolderId = root;
            var pageData = documents.ToPagedList(page, pagesize);
            return View(pageData);
        }

        public ActionResult UploadFile(string doclib, int? root) // optionally receive values specified with Html helper
        {
            

            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                //save item
                var file = Request.Files[i];

                var lib = libService.GetByName(doclib);
                if (lib == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var name = Path.GetFileNameWithoutExtension(file.FileName);
                var existing = service.GetAll().FirstOrDefault(p => p.ParentId == root && p.Name == name);

                if (existing!= null)
                {
                    name = name + DateTime.Now.Ticks.ToString();
                }

                string uuid = PasswordGenerator.GetUniqueKey(20);

                var st = BlobStogareSaver.StoreFile(x =>
                {
                    x.File = Request.Files[i];
                    //note how we are adding an additional value to be posted with delete request
                    //and giving it the same value posted with upload
                    x.DeleteUrl = Url.Action("DeleteFile", new { entityId = 0 });
                   // x.StorageDirectory = Server.MapPath("~/Content/uploads");
                    x.StorageDirectory = lib.BlobContainer;
                    
                    x.UrlPrefix = GetFolderPath<string>(root) as string;// this is used to generate the relative url of the file
                    //overriding defaults
                    x.FileName = Request.Files[i].FileName;// default is filename suffixed with filetimestamp
                    x.ThrowExceptions = true;//default is false, if false exception message is set in error property
                });
                //Update BlobURL after upload.

                Document documentItem = new Document()
                {
                    Name = name,
                    DocumentLibraryId = lib.Id,
                    Extension = Path.GetExtension(file.FileName),
                    FileSize = file.InputStream.Length,
                    ItemType = (int)DocumentType.File,
                    Created = DateTime.Now,
                    ParentId = root,
                    BlobUrl = st.url,
                    Uuid = uuid,
                    Modified = DateTime.Now,
                    CreatedBy = SessionManager.CurrentUser.Id

                };

                service.Add(documentItem);
                string ext =Path.GetExtension(st.url);
                if(ext ==".jpg" || ext==".png" || ext == ".gif" || ext == ".bmp") {
                    st.thumbnailUrl = st.url;

                }
                else
                {
                    st.thumbnailUrl = "/Content/Images/nopreview.jpg";
                    st.url = st.thumbnailUrl;
                }
                st.deleteUrl =  Url.RouteUrl(KumoConstants.DOCUMENT_ROOT_ROUTE_NAME, new { doclib = lib.Name
                    , action = "DeleteFile", uuid = uuid});
                st.url = Url.RouteUrl(KumoConstants.DOCUMENT_ROOT_ROUTE_NAME, new { doclib = lib.Name
                    , action = "DownloadFile", uuid = uuid});
                statuses.Add(st);
            }

            //statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            //todo: add additional code to generate thumbnail for videos, associate files with entities etc

            //adding thumbnail url for jquery file upload javascript plugin
            //statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80"); // uses ImageResizer httpmodule to resize images from this url

            //setting custom download url instead of direct url to file which is default
            //statuses.ForEach(x => x.url = Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));


            //server side error generation, generate some random error if entity id is 13
            //if (entityId == 13)
            //{
            //    var rnd = new Random();
            //    statuses.ForEach(x =>
            //    {
            //        //setting the error property removes the deleteUrl, thumbnailUrl and url property values
            //        x.error = rnd.Next(0, 2) > 0 ? "We do not have any entity with unlucky Id : '13'" : String.Format("Your file size is {0} bytes which is un-acceptable", x.size);
            //        //delete file by using FullPath property
            //        if (System.IO.File.Exists(x.FullPath)) System.IO.File.Delete(x.FullPath);
            //    });
            //}

            var viewresult = Json(new { files = statuses });
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            return viewresult;
        }

        

        private object GetFolderPath<T>(int? root) where T: class
        {
            List<Document> results = new List<Document>();
            if (!root.HasValue) return null;

            else
            {
                string path = "";
                Document doc = null;
                do
                {
                    doc = service.GetById(doc == null ? root.Value : doc.ParentId.Value);
                    results.Insert(0, doc);
                    path = doc.Name + "/" + path;

                }
                while (doc != null && doc.ParentId.HasValue);


                if (typeof(T) == typeof(string)) return path;

                return results;
                return path;
            }
            
            //return string.Empty;

        }



        // GET: mis/newfolder
        public ActionResult NewFolder(string doclib, int? root )
        {
            var library = libService.GetAll().FirstOrDefault(p => p.Name == doclib);

            if (library == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //find
            return View(new FolderViewModel()
            {
                LibraryId = library.Id,
                ParentId = root ,
                
            });
        }

        // POST: mis/NewFolder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewFolder([Bind(Include = "Name,ParentId,LibraryId")] FolderViewModel folder)
        {
            if (ModelState.IsValid)
            {
                Document item = new Document()
                {
                    Name = folder.Name, 
                    ParentId = folder.ParentId,
                    DocumentLibraryId = folder.LibraryId,
                    ItemType = (int)DocumentType.Folder,
                    Extension = "folder",
                    Uuid = PasswordGenerator.GetUniqueKey(12),
                    BlobUrl = folder.Name,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreatedBy = SessionManager.CurrentUser.Id
                };


                service.Add(item);
                return RedirectToAction("Index", new { root = folder.ParentId });
            }

            return View(folder);
        }

        public ActionResult Upload(int? root)
        {
            if (root.HasValue)
            {
                Document item = service.GetAll().FirstOrDefault(p => p.Id == root);
                if (item == null)
                {
                    return HttpNotFound();
                }
            }

            return View(new ReportDocumentViewModel()
            {
                ParentId = root
            });
        }

        //private MultipartFileStreamProvider GetMultipartProvider()
        //{
        //    const string tempUploadFolder = "~/App_Data/Temp/FileUploads";
        //    var root = HttpContext.Current.Server.MapPath(tempUploadFolder);
        //    Directory.CreateDirectory(root);

        //    return new MultipartFileStreamProvider(root);
        //}

        //private string GetDeserializedFileName(MultipartFileData fileData)
        //{
        //    var fileName = GetFileName(fileData).Split('\\');
        //    var lastPart = fileName.Last();
        //    if (!string.IsNullOrEmpty(lastPart))
        //    {
        //        if (lastPart[0] != '\"')
        //        {
        //            lastPart = "\"" + lastPart;
        //        }
        //        return JsonConvert.DeserializeObject(lastPart).ToString();
        //    }
        //    else
        //        return string.Empty;
        //}
        //private string GetFileName(MultipartFileData fileData)
        //{
        //    return fileData.Headers.ContentDisposition.FileName;
        //}

        // GET: mis/Edit/5
        public ActionResult Edit(string doclib, string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document item = service.GetByUUID(uuid);
            if (item == null)
            {
                return HttpNotFound();
            }
            var carModel = new DocumentEditViewModel();
            carModel.CopyFrom<DocumentEditViewModel>(item);
            carModel.Library = item.DocumentLibrary;
            
            //InitialForgeinData(createdBy: item.CreatedBy);

            return View(carModel);
        }

        // POST: mis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParentId,")] DocumentEditViewModel item, string uuid)
        {
            if (ModelState.IsValid)
            {
                Document stored = service.GetById(item.Id);
                if (stored == null)
                {
                    return HttpNotFound();
                }
                stored.Name = item.Name;

                service.Update(stored);
                
                return RedirectToRoute(KumoConstants.DOCUMENT_ROOT_ROUTE_NAME, new { doclib = stored.DocumentLibrary.Name, uuid = uuid, action = "index", root = item.ParentId });
            }

            //InitialForgeinData(createdBy: document.AssignedTo);
            return View(item);
        }
        public ActionResult DownloadFile(string doclib, string uuid)
        {
            var library = libService.GetByName(doclib);
            if (library == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string container = library.BlobContainer;
            BlobStogareSaver saver  = new BlobStogareSaver(container);
            var document = service.GetByUUID(uuid);
             var mineType = "";
            var fileBytes = new byte[1];
            string filename = "";

            if (document != null) 
            { 
                //if (document == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
                filename = document.Name + document.Extension;
                if (document.ItemType == (int)DocumentType.File)
                {
                    //GetMimeType(document.Extension)
                    fileBytes = saver.DownloadToByteArray(GetBlogFilename(document.BlobUrl));

                }
                else
                {
                    List<Document> documents =new List<Document>();
                    //get all document in this folder
                    documents= service.GetDocuments(document.Id);
                
                        //var file = saver.DownloadToByteArray(GetBlogFilename(item.BlobUrl));

                   fileBytes = GenerateZipFile(saver, documents);
                     filename = document.Name + ".zip";
                }
            

            }
            else{
                //Download whole folder 
                if (uuid == library.Name)
                {
                    var documents= service.GetAll().Where(p=>p.DocumentLibraryId == library.Id && p.ItemType == (int)DocumentType.File).ToList();
                
                        //var file = saver.DownloadToByteArray(GetBlogFilename(item.BlobUrl));

                     fileBytes = GenerateZipFile(saver, documents);
                     filename = library.Name + ".zip";
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            mineType = MimeMapping.GetMimeMapping(filename);
            return File(fileBytes, mineType, filename);
        
        
    
        }




        private byte[] GenerateZipFile(BlobStogareSaver saver, List<Document> documents)
        {
            byte[] zipData;

            using (var zipToOpen = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create, true))
                {
                    //var directory = archive.CreateDirectory("context");
                    //var entry = directory.CreateEntry(context);
                    //var stream = entry.Open();

                    foreach (var item in documents)
                    {
                        string folder = GetFolderPath<string>(item.ParentId) as string;

                        string zipFilename = folder + item.Name + item.Extension;
                        //GetZipEntryFileName(item.BlobUrl)
                        ZipArchiveEntry fileEntry = archive.CreateEntry(zipFilename, CompressionLevel.Optimal);
                        using (Stream writer = fileEntry.Open())
                        {


                            saver.DownloadToStream(GetBlogFilename(item.BlobUrl), writer);
                        }
                    }

                }

                zipToOpen.Seek(0, SeekOrigin.Begin);
                zipData = zipToOpen.ToArray();
            }
            return zipData;
        }


        private string GetZipEntryFileName(string uriFormat)
        {
            return GetBlogFilename(uriFormat).Replace("/", "\\");
        }

        /// <summary>
        /// This function accecept the input as full URI to Blob, then retrn the blob name without container.
        /// </summary>
        /// <param name="blobURL">https://kumoeip.blob.core.windows.net/dev-project-documents/Project X/Untitled.png</param>
        /// <returns>/Project X/Untitled.png</returns>
        private string GetBlogFilename(string blobURL)
        {
            if (blobURL.StartsWith("http") && blobURL.Contains("://"))
            {
                //https://kumoeip.blob.core.windows.net/dev-project-documents/Project X/Untitled.png
                string relativePath = blobURL.Substring(blobURL.IndexOf("/", 11));
                string container = relativePath.Substring(0, relativePath.IndexOf("/", 1) + 1);

                relativePath = relativePath.Replace(container, string.Empty);
                return relativePath;

            }
            return blobURL;
        }

        public string GetMimeType(string ext, string defaultType="application/octet-stream")
        {
            // who would load the file on every method call?  That's just dumb
            var mimes = XElement.Load(Server.MapPath("~/App_Data/Minetypes.xml"));
            var result = from x in mimes.Elements()
                         where Contains(x, ext)
                         select x.Attribute("Type");

            var minetype = result.FirstOrDefault();
            return minetype != null ? minetype.ToString() : defaultType;
        }

        public bool Contains(XElement el, string ext)
        {
            return el.Attribute("Extensions").Value.Contains(ext);
        }

        // GET: mis/Delete/5
        public ActionResult Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document item = service.GetByUUID(uuid);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost]
        public ActionResult DeleteFile(string uuid)
        {
            DeleteFileOrFolderByUUID(uuid);

            var viewresult = Json(new { error = String.Empty });
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            return viewresult; // trigger success

        }

        // POST: Calendar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string uuid)
        {
            DeleteFileOrFolderByUUID(uuid);

            return RedirectToAction("Index");
        }

        private void DeleteFileOrFolderByUUID(string uuid)
        {
            Document item = service.GetByUUID(uuid);
            List<Document> allDocuments = new List<Document>();

            if (item.ItemType == (int)DocumentType.Folder)
            {
                allDocuments = service.GetDocuments(item.Id, true);
            }
            allDocuments.Add(item);

            BlobStogareSaver azureBlobStogare = new BlobStogareSaver(item.DocumentLibrary.BlobContainer);

            foreach (var docItem in allDocuments)
            {
                try
                {
                    if (docItem.ItemType == (int)DocumentType.File)
                    {
                        azureBlobStogare.DeleteBlob(GetBlogFilename(docItem.BlobUrl));
                    }

                    service.Delete(docItem);
                }
                catch (Exception ex)
                {


                }
            };
        }
    }
}
