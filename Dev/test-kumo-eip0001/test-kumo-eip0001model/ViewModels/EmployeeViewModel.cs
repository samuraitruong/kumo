using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model.ViewModels
{
    public class EmployeeViewModel 
    {
        [Display(ResourceType = typeof(StringResources), Name = "EmployeeName")]
        public string Name {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "FirstName")]
        public string Firstname { get; set; }
        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "LastName")]
        public string Lastname { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "Fullname")]
        public string Fullname { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "JobTitle")]
        public string JobTitle { get; set; }

       
        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "HomeAddress")]
        public string Address { get; set; }

        public int Id { get; set; }
        public string EmployeeId { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "DateOfBirth")]      
        //[DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        
        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "EffectiveDate")]
        //[DataType( System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "DirectPhone")]
       // [RegularExpression(@"^\+([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{6})$", ErrorMessage = "Entered direct phone number format is not valid. Please enter +country code phone number with a space between country code and phone number. E.g. +61 1234 123456")]
        public string DirectPhone { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "MobileNumber")]
        //[RegularExpression(@"^\+\(?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{6})$", ErrorMessage = "Entered phone number format is not valid. Please enter +country code phone number with a space between country code and phone number.\r\n E.g. +61 1234 123456")]
         //[RegularExpression(@"^\+([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{6})$", ErrorMessage = "Entered phone number format is not valid. Please enter +country code phone number with a space between country code and phone number. E.g. +61 1234 123456")]
        public string Phone { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        [DataType( System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$", 
            ErrorMessageResourceType = typeof(StringResources), 
            ErrorMessageResourceName = "EmailNotValid")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "LineManager")]
        public int? LineManager { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "Department")]
        public int DepartmentID { get; set; }

       

        [Required]
        public string Status { get; set; }
    }
}
