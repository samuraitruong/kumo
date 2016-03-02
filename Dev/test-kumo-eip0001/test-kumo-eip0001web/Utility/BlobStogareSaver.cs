using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MvcFileUploader;
using MvcFileUploader.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001web.Utility
{
    
    public class BlobStogareSaver 
    {
#region Blob Stogare Code
        //these variables are used throughout the class
        string ContainerName { get; set; }
        CloudBlobContainer cloudBlobContainer { get; set; }

        public BlobStogareSaver(string containerName)
        {
            InitialBlobAccess(ConfigurationManager.AppSettings["BlobAccountId"], ConfigurationManager.AppSettings["BlobAccessKey"], containerName);
        }
        //this is the only public constructor; can't use this class without this info
        public  BlobStogareSaver(string storageAccountName, string storageAccountKey, string containerName)
        {
            InitialBlobAccess(storageAccountName, storageAccountKey, containerName);
        }

        private void InitialBlobAccess(string storageAccountName, string storageAccountKey, string containerName)
        {
            cloudBlobContainer = SetUpContainer(storageAccountName, storageAccountKey, containerName);
            ContainerName = containerName;

            //CloudBlobContainer startCloudBlobContainer =
            // SetUpContainer(storageAccountName, storageAccountKey, containerName);
            //just in case, check to see if the container exists,
            //  and create it if it doesn't
            cloudBlobContainer.CreateIfNotExists();

            //set access level to "blob", which means user can access the blob 
            //  but not look through the whole container
            //this means the user must have a URL to the blob to access it
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            cloudBlobContainer.SetPermissions(permissions);
        }

        public BlobStogareSaver(string blobConnectionString, string containerName)
        {

        }
        /// <summary>
        /// set up references to the container, etc.
        /// </summary>
        private CloudBlobContainer SetUpContainer(string storageAccountName,
          string storageAccountKey, string containerName)
        {
            string connectionString = string.Format(@"DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
            storageAccountName, storageAccountKey);

            //get a reference to the container where you want to put the files
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            return cloudBlobContainer;
        }

        public void RunAtAppStartup(string storageAccountName,
          string storageAccountKey, string containerName)
        {
            CloudBlobContainer startCloudBlobContainer = 
            SetUpContainer(storageAccountName, storageAccountKey, containerName);
            //just in case, check to see if the container exists,
            //  and create it if it doesn't
             cloudBlobContainer.CreateIfNotExists();

            //set access level to "blob", which means user can access the blob 
            //  but not look through the whole container
            //this means the user must have a URL to the blob to access it
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            cloudBlobContainer.SetPermissions(permissions);     
        }

        internal string UploadFromFile(string localFilePath)
        {
            string status = string.Empty;
            CloudBlockBlob blob = 
            cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(localFilePath));
            blob.UploadFromFile(localFilePath, FileMode.Open);
            status = "Uploaded successfully.";
            return status;
        }

        internal string UploadText(string textToUpload, string targetFileName)
        {
            string status = string.Empty;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(targetFileName);
            blob.UploadText(textToUpload);
            status = "Finished uploading.";
            return status;
        }

        internal string UploadFromByteArray(Byte[] uploadBytes, string targetFileName)
        {
            string status = string.Empty;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(targetFileName);
            blob.UploadFromByteArray(uploadBytes, 0, uploadBytes.Length);
            status = "Uploaded byte array successfully.";
            return status;
        }

        public string UploadFromStream(Stream stream, string targetBlobName)
        {
            string status = string.Empty;
            //reset the stream back to its starting point (no partial saves)
            stream.Position = 0;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(targetBlobName);
            blob.UploadFromStream(stream);
            status = "Uploaded successfully.";
            return blob.StorageUri.PrimaryUri.ToString();

            return status;
        }

        internal string DownloadFile(string blobName, string downloadFolder)
        {
            string status = string.Empty;
            CloudBlockBlob blobSource = cloudBlobContainer.GetBlockBlobReference(blobName);
            if (blobSource.Exists())
            {
            //blob storage uses forward slashes, windows uses backward slashes; do a replace
            //  so localPath will be right
            string localPath = Path.Combine(downloadFolder, blobSource.Name.Replace(@"/", @"\"));
            //if the directory path matching the "folders" in the blob name don't exist, create them
            string dirPath = Path.GetDirectoryName(localPath);
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            blobSource.DownloadToFile(localPath, FileMode.Create);
            }
            status = "Downloaded file.";
            return status;
        }

        internal Byte[] DownloadToByteArray(string targetFileName)
        {
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(targetFileName);
            //you have to fetch the attributes to read the length
            blob.FetchAttributes();
            long fileByteLength = blob.Properties.Length;
            Byte[] myByteArray = new Byte[fileByteLength];
            blob.DownloadToByteArray(myByteArray, 0);
            return myByteArray;
        }

        public string DownloadToStream(string sourceBlobName, Stream stream)
        {
            string status = string.Empty;
            try
            {
                CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(sourceBlobName);
                blob.DownloadToStream(stream);
            }
            catch (Exception ex)
            {
                var data = Encoding.Default.GetBytes(StringResources.FileNotFoundOnBlob);
                stream.Write(data, 0, data.Length);
                stream.Flush();
                //throw;
            }
           
            status = "Downloaded successfully";
            return status;
        }

        internal string RenameBlob(string blobName, string newBlobName)
        {
            string status = string.Empty;

            CloudBlockBlob blobSource = cloudBlobContainer.GetBlockBlobReference(blobName);
            if (blobSource.Exists())
            {
                CloudBlockBlob blobTarget = cloudBlobContainer.GetBlockBlobReference(newBlobName);
                blobTarget.StartCopyFromBlob(blobSource);
                blobSource.Delete();
            }

            status = "Finished renaming the blob.";
            return status;
        }

        //if the blob is there, delete it 
        //check returning value to see if it was there or not
        internal string DeleteBlob(string blobName)
        {
            string status = string.Empty;
            CloudBlockBlob blobSource = cloudBlobContainer.GetBlockBlobReference(blobName);
            bool blobExisted = blobSource.DeleteIfExists();
            if (blobExisted)
            {
                status = "Blob existed; deleted.";
            }
            else
            {
                status = "Blob did not exist.";
            }
            return status;
        }

        /// <summary>
        /// parse the blob URI to get just the file name of the blob 
        /// after the container. So this will give you /directory1/directory2/filename if it's in a "subfolder"
        /// </summary>
        /// <param name="theUri"></param>
        /// <returns>name of the blob including subfolders (but not container)</returns>
        private string GetFileNameFromBlobURI(Uri theUri, string containerName)
        {
            string theFile = theUri.ToString();
            int dirIndex = theFile.IndexOf(containerName);
            string oneFile = theFile.Substring(dirIndex + containerName.Length + 1,
                theFile.Length - (dirIndex + containerName.Length + 1));
            return oneFile;
        }

        internal List<string> GetBlobList()
        {
            List<string> listOBlobs = new List<string>();
            foreach (IListBlobItem blobItem in cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.All))
            {
                string oneFile = GetFileNameFromBlobURI(blobItem.Uri, ContainerName);
                listOBlobs.Add(oneFile);
            }
            return listOBlobs;
        }

        internal List<string> GetBlobListForRelPath(string relativePath)
        {
            //first, check the slashes and change them if necessary
            //second, remove leading slash if it's there
            relativePath = relativePath.Replace(@"\", @"/");
            if (relativePath.Substring(0, 1) == @"/")
            relativePath = relativePath.Substring(1, relativePath.Length - 1);

            List<string> listOBlobs = new List<string>();
            foreach (IListBlobItem blobItem in 
            cloudBlobContainer.ListBlobs(relativePath, true, BlobListingDetails.All))
            {
                string oneFile = GetFileNameFromBlobURI(blobItem.Uri, ContainerName);
                listOBlobs.Add(oneFile);
            }
            return listOBlobs;
        }
#endregion

        public static List<ViewDataUploadFileResult> StoreFiles(IEnumerable<MvcFileSave> mvcFiles)
        {
            return mvcFiles.Select(x => StoreFile(delegate(MvcFileSave f)
            {
                if (f == null) throw new ArgumentNullException("MvcFileSave");
                f = x;
            })).ToList();
        }


        public static ViewDataUploadFileResult StoreFile(Action<MvcFileSave> action)
        {
            var mvcFile = new MvcFileSave();
            mvcFile.FileTimeStamp = DateTime.Now.ToUniversalTime();
            action(mvcFile);

            //Save data to Azure

            BlobStogareSaver dataStogare = new BlobStogareSaver(mvcFile.StorageDirectory);

            //TODO : Append time stamp to make sure the file on blob is unique
            string remotePath = mvcFile.UrlPrefix + Path.GetFileNameWithoutExtension(mvcFile.FileName)  +"-"+mvcFile.FileTimeStamp.ToString("yyyyMMddhhmmss") + Path.GetExtension(mvcFile.FileName);

            string blobURL = dataStogare.UploadFromStream(mvcFile.File.InputStream, remotePath);

            ViewDataUploadFileResult status;

            var dirInfo = new DirectoryInfo(mvcFile.StorageDirectory);
            var file = mvcFile.File;
            var fileNameWithoutPath = Path.GetFileName(mvcFile.File.FileName);
            var fileExtension = Path.GetExtension(fileNameWithoutPath);
            var fileName = Path.GetFileNameWithoutExtension(Path.GetFileName(mvcFile.File.FileName));
            var genName = fileName + "-" + mvcFile.FileTimeStamp.ToFileTime();
            var genFileName = String.IsNullOrEmpty(mvcFile.FileName) ? genName + fileExtension : mvcFile.FileName;// get filename if set
            var fullPath = Path.Combine(mvcFile.StorageDirectory, genFileName);

            try
            {
                var viewDataUploadFileResult = new ViewDataUploadFileResult()
                {
                    name = fileNameWithoutPath,
                    SavedFileName = genFileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    //url = mvcFile.UrlPrefix + "/" + genFileName,
                    url = blobURL,
                    //delete_url = Url.Action("DeleteFile", new { fileUrl = "/"+storageRoot+"/" + genFileName }),
                    //thumbnail_url = thumbUrl + "?width=100",
                    deleteType = "POST",
                    Title = mvcFile.File.FileName,

                    //for controller use
                    FullPath = dirInfo.FullName + "/" + genFileName
                };

                //add delete url                           
                mvcFile.AddFileUriParamToDeleteUrl("fileUrl", viewDataUploadFileResult.url);
                viewDataUploadFileResult.deleteUrl = mvcFile.DeleteUrl;


                status = viewDataUploadFileResult;

                //mvcFile.File.SaveAs(fullPath);
            }
            catch (Exception exc)
            {
                if (mvcFile.ThrowExceptions)
                    throw;

                status = new ViewDataUploadFileResult()
                {
                    error = exc.Message,
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType
                };
            }

            return status;
        } 
    

    }

}