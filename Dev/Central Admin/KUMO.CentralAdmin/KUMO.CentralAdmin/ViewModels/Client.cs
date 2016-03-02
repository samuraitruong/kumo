using KUMO.CentralAdmin.Model.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Model
{
     [MetadataType(typeof(ClientMetadata))]
    public partial class Client
    {
         public int[] SelectedComponents { get; set; }

         [Display(Name = "Component", ResourceType = typeof(ClientResources))]
         public string ComponentNames { get; set; }
         public List<Component> Components { get; set; }
    }


    public class ClientMetadata{

        [Required]
        [Display(Name = "ClientName", ResourceType = typeof(ClientResources))]
        public string ClientName;

        [Required]
        [RegularExpression("^[a-zA-Z]+[a-zA-Z0-9_-]*$", ErrorMessage="Invalid database name.")]
        [Display(Name = "DBName", ResourceType = typeof(ClientResources))]
        public string DBName;

        [Required]
        [RegularExpression("^[a-zA-Z]+[a-zA-Z0-9_-]*$", ErrorMessage = "Invalid database user name")]
        [Display(Name = "DBUser", ResourceType = typeof(ClientResources))]
        public string DBUser;


        [Required]
        [Display(Name = "DBPassword", ResourceType = typeof(ClientResources))]
        public string DBPassword;


        [Required]
        [Display(Name = "DBServer", ResourceType = typeof(ClientResources))]
        public string DBServer;

    }

    public class CompanyAdminViewModel
    {

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "FirstName")]
        public string Firstname { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "LastName")]
        public string Lastname { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
