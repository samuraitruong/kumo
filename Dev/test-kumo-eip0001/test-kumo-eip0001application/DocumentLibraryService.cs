using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001repositories;

namespace test_kumo_eip0001application
{
    public class DocumentLibraryService : ServiceBase<DocumentLibrary>
    {
        public DocumentLibrary GetByName(string name)
        {
            return GetAll().FirstOrDefault(p => p.Name == name);
        }
        public DocumentLibraryService()
        {
        }


       
    }
}
