using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model.ViewModels
{
    public class ReportDocumentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(ResourceType = typeof(MISResource), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "ItemType")]
        public int ItemType { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "ParentId")]
        public Nullable<int> ParentId { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "Created")]
        public Nullable<System.DateTime> Created { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "Extension")]
        public string Extension { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "Uuid")]
        public string Uuid { get; set; }

        [Display(ResourceType = typeof(MISResource), Name = "FileSize")]
        public Nullable<long> FileSize { get; set; }

    }
}
