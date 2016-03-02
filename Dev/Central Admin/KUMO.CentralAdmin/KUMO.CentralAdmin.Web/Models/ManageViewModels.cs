using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using KUMO.CentralAdmin.Model;
using KUMO.CentralAdmin.Model.Resources;

namespace KUMO.CentralAdmin.Web.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public User CurrentUser { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "PasswordLengthMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "NewPasswordAndConfirmDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "PasswordLengthMessage", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "NewPassword")]
        [RegularExpression(CAConstants.STRONG_PASSWORD_PATTERN, ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "PasswordDoNotMatch")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "NewPasswordAndConfirmDoNotMatch")]
        public string ConfirmPassword { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(ResourceType = typeof(StringResources), Name = "PhoneNumber")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(ResourceType = typeof(StringResources), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}