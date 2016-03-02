using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web.Controllers
{
    public class TestController : KumoBaseController
    {
        public ActionResult Metadata()
        {
            return View(new EmployeeViewModel());
        }

        private string GetBlogFilename(string blobURL)
        {
            if (blobURL.StartsWith("http") && blobURL.Contains("://"))
            {
                //https://kumoeip.blob.core.windows.net/dev-project-documents/Project X/Untitled.png
                string relativePath = blobURL.Substring(blobURL.IndexOf("/", 11));
                string container = relativePath.Substring(0, relativePath.IndexOf("/",1)+1);

                relativePath = relativePath.Replace(container, string.Empty);
                return relativePath;

            }
            return blobURL;
        }

        // GET: Test
        public ActionResult Index()
        {
            long t = 12345667666;

            //return Content(t.ToFileSize());
            string testURL = "https://kumoeip.blob.core.windows.net/dev-project-documents/Untitled.png";

            //return Content(GetBlogFilename(testURL));

            // test blob create container

             
            BlobStogareSaver saver = new BlobStogareSaver(ConfigurationManager.AppSettings["BlobAccountId"], ConfigurationManager.AppSettings["BlobAccessKey"], "testcontainer");
            //saver.RunAtAppStartup(ConfigurationManager.AppSettings["BlobAccountId"], ConfigurationManager.AppSettings["BlobAccessKey"], "testcontainer");
            
            string result;
            using (var file = System.IO.File.OpenRead("d:\\test.pdf"))
            {
                 result = saver.UploadFromStream(file, "Folder 4\\test3.pdf");
                
            }
            return Content(result);
        }
    }
}