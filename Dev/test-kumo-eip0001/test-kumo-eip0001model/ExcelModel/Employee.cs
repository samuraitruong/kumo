using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001model
{

    [MetadataType(typeof(EmployeeMeta))]
    public partial class Employee
    {
        [ExcelIgnored]
        public Customer Customer { get; set; }

        // [NotMapped]
        public string _Department
        {
            get
            {
                return Department.Name;
            }
        }

        public string _Manager
        {
            get
            {
                return Manager != null? Manager.Fullname:"";
            }
        }

        public string GetManager()
        {
            return "Aaaaaaaaaaa";
        }
        //public  ICollection<Customer> Customers { get; set; }
        //[ExcelIgnored]
        //public  ICollection<Employee> Employees { get; set; }

    }
    public class EmployeeMeta{
        
        [ExcelIgnored]
        public int DepartmentID { get; set; }

         [ExcelIgnored]
        public Nullable<int> LineManagerId { get; set; }

         [ExcelIgnored]
         public string Name { get; set; }

        [DataType(DataType.MultilineText)]

         public string Skills{ get; set; }

        [Required]
        [Display(Name="Fistname", ResourceType = typeof(HRMResources))]
        public string Firstname;
        [Required]
        [Display(Name = "Lastname", ResourceType = typeof(HRMResources))]

        public string Lastname;
        
        
        public string Fullname;
        public string Gender;
        [UIHint("DOB")]
        public System.DateTime DateOfBirth;
        public string IdentificationNumber;


        [UIHint("Metadata")]
        [AdditionalMetadata("metadata", typeof(IdentityType))]
        public int IdentityType {get;set;}

        public string MobileNumber;
        public string WorkNumber;
        public string HomeNumber;
        public string Email;
        public string Address;
        public string EmployeeId;
        public int JobTitleId;


        public int EmploymentStatusId;
        public bool Current;
        public System.DateTime EffectiveDate;
        public Nullable<System.DateTime> EndDate;
        public Nullable<System.DateTime> TerminaiondDate;
        public int WorkPassTypeId;
        public Nullable<System.DateTime> WorkPassExpiryDate;
        public Nullable<int> NationalityId;
        public string UserId;
        public decimal Salary;
        public int PaymentFrequencyId;
        public int PaymentModeId;
        public int BankId;
        public string AccountName;
        public string MedicalIsuranceNumber;
        public int HighestQualificationId;

        public string EmergencyContactFirstName;
        public string EmergencyContactLastName;
        public string EmergencyContactFullName;
        public string EmergencyContactMobileNumber;
        public string EmergencyContactWorkNumber;
        public string EmergencyContactEmailAddress;
        public int EmergencyContactRelationshipId;


    }
}
