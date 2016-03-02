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
    public class CustomerViewModel 
    {
        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "Website")]
        public string Website { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "FaxNumber")]
        public string FaxNumber { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "CompanyAddress")]
        public string CompanyAddress { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "PhoneNumber")]
        //[RegularExpression(@"^\+\(?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{6})$", ErrorMessage = "Entered phone number format is not valid. Please enter +country code phone number with a space between country code and phone number.")]
        public string Phone { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "CompanyEmailAddress")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        public string CompanyEmailAddress { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "Source")]
        public string Source { get; set; }

        [Required]
        [Display(ResourceType = typeof(CRMResource), Name = "LeadType")]
        public string LeadType { get; set; }

        public int Id { get; set; }

        public string CustomerId { get; set; }

        [Display(ResourceType = typeof(CRMResource), Name = "AssignedTo")]
        [Required(ErrorMessageResourceType = typeof(CRMResource), ErrorMessageResourceName = "EmployeeRequiredMessage")]
        public int AssignedTo { get; set; }

        [Display(ResourceType = typeof(CRMResource), Name = "Industry")]
        [Required(ErrorMessageResourceType = typeof(CRMResource), ErrorMessageResourceName = "IndustryRequiredMessage")]
        public int IndustryId { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(ResourceType = typeof(CRMResource), Name = "Description")]
        public string Description { get; set; }
    }
}
