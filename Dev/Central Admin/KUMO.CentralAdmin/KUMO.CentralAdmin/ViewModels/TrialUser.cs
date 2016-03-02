using KUMO.CentralAdmin.Model.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Model
{
    //[MetadataType(typeof(TrialUserMetadata))]
    public  class TrialUserViewModel
    {
        [Required]
        [Display(Name = "Company", ResourceType = typeof(TrialUserResources))]
        public string Company {get;set;}

        [Required]
        public string Firstname{get;set;}
        [Required]
        public string Lastname{get;set;}
        [Required]
        public string Email{get;set;}
        [Required]
        public string Phone{get;set;}
    }


}
