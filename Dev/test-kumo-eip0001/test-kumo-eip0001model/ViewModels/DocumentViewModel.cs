using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model.ViewModels
{
    public class DocumentEditViewModel
    {
        public int? ParentId { get; set; }
        public DocumentLibrary Library{ get; set; }
        public string Name { get; set; }

        public string Extension { get; set; }

        public int Id { get; set; }

        public string Uuid { get; set; }
    }

    public class FolderViewModel{
        [Required]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public int LibraryId { get; set; }
    }
}
