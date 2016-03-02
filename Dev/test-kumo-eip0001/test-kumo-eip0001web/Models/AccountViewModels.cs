using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Resources;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "RememberBrowser")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(StringResources), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "ConfirmPassword")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Display(ResourceType = typeof(StringResources), Name = "Company")]
        public string Company { get; set; }


        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "FirstName")]
        public string Firstname { get; set; }



        [Required]
        [Display(ResourceType = typeof(StringResources), Name = "LastName")]
        public string Lastname { get; set; }

        public string[] SelectedPermissions { get; set; }
        public List<SystemAction> SystemActions { get; set; }
        public List<Component> SelectedComponents { get; set; }

    }

  
    public class ResetPasswordViewModel
    {
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "PasswordLengthMessage", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "CreateNewPassword")]
        [RegularExpression(KumoConstants.STRONG_PASSWORD_PATTERN, ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "PasswordDoNotMatch")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(StringResources), Name = "ConfirmNewPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(StringResources), ErrorMessageResourceName = "PasswordAndConfirmDoNotMatch")]
        public string ConfirmPassword { get; set; }


        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9]+[a-zA-Z0-9-]*\\.)+[a-zA-Z]{2,6}$",
            ErrorMessageResourceType = typeof(StringResources),
            ErrorMessageResourceName = "EmailNotValid")]
        [Display(ResourceType = typeof(StringResources), Name = "EmailAddress")]
        public string Email { get; set; }
    }
}
