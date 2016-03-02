using KUMO.CentralAdmin.Model.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Model
{
    [MetadataType(typeof(EIPUserMetadata))]
    public partial class EIPUser
    {
    }

    public class EIPUserMetadata
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        [Display(Name = "Email", ResourceType = typeof(EIPUserResources))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(EIPUserResources))]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "LastName", ResourceType = typeof(EIPUserResources))]
        public string Lastname { get; set; }
    }
}
