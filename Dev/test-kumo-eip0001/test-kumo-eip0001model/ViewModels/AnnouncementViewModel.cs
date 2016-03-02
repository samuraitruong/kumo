using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model.ViewModels
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(CCSResource), Name = "Title")]
        public string Title { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(ResourceType = typeof(CCSResource), Name = "Body")]
        public string Body { get; set; }

        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
        public System.DateTime PublishedDate { get; set; }
    }
}
