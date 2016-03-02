using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;

using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001repositories;
using System.Linq.Dynamic;

namespace test_kumo_eip0001application
{
    public class DocumentService : ServiceBase<Document>
    {
        public DocumentService()
        {
        }




        public Document GetByUUID(string uuid)
        {
            return GetAll()
                
                        .FirstOrDefault(p => p.Uuid == uuid);
        }

        public List<Document> GetDocuments(int parentFolderId, bool includeFolder=false)
        {
            var result = new List<Document>();
            var currentFiles = GetAll().Where(p => p.ParentId == parentFolderId && p.ItemType == (int)DocumentType.File).ToList();
            result.AddRange(currentFiles);

            var subFolders =  GetAll().Where(p => p.ParentId == parentFolderId && p.ItemType == (int)DocumentType.Folder).ToList();

            foreach (var item in subFolders)
	        {
                
                result.AddRange(GetDocuments(item.Id, includeFolder));
	        }
            if (includeFolder) result.AddRange(subFolders);

            return result;
        }
    }
}
