using KUMO.CentralAdmin.Model.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Model.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "FirstName")]
        public string Firstname { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "LastName")]
        public string Lastname { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "Company")]
        public string Company { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "PhoneNumber")]
        //[RegularExpression(@"^\+\(?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{6})$", ErrorMessage = "Entered phone number format is not valid. Please enter +country code phone number with a space between country code and phone number.")]
        public string Phone { get; set; }
    }
}
