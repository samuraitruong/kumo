﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using test_kumo_eip0001web.Models;
using test_kumo_eip0001application;
using System.Web.Security;
using test_kumo_eip0001model;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using test_kumo_eip0001web.Mailers;
using test_kumo_eip0001model.ViewModels;
using test_kumo_eip0001web.Utility;
using PagedList;
using System.Linq.Dynamic;
using System.Net;
using System.Configuration;
using RestSharp;
using Microsoft.AspNet.Identity.EntityFramework;

namespace test_kumo_eip0001web.Controllers
{
    [DebuggerDisplayAttribute("{Id}-{FirstName}")]
    public class Person
    {
        [Display(Order = 1, Name = "First")] //<--- set custom title
        public string FirstName { get; set; }
        [Display(Order = 2, Name = "Last")]
        public string LastName { get; set; }
        [Display(Order = 0)] //<--- specify order
        public int Id { get; set; }
        public string Email { get; set; }
        [Display(AutoGenerateField = false)]
        public DateTime BirthDate { get; set; }
        public string DateOfBirth { get { return BirthDate.ToShortDateString(); } } //<--- title split camel-case
        public string Location { get; set; }

        [HiddenInput(DisplayValue = false)] //<--- ignore field (method 1)
        public string HiddenField1 { get; set; }
        [Display(AutoGenerateField = false)] //<--- ignore field (method 2)
        public string HiddenField2 { get; set; }
    }

    [Authorize]
    public class AccountController : KumoBaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private AccountService accountService;
        private UserActionService userActionService;

        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        public AccountController()
        {
            accountService = new AccountService();
            userActionService = new UserActionService();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

            if (!string.IsNullOrEmpty(SessionManager.PrivateTenantConnectionString))
            {
                var db = ApplicationDbContext.Create(SessionManager.PrivateTenantConnectionString);

                UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

                SignInManager = new ApplicationSignInManager(UserManager, HttpContext.GetOwinContext().Authentication);
            }

            accountService = new AccountService();
            userActionService = new UserActionService();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {

                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;

            return PartialView();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var connString = TenantHelper.GetConnectionString(model.Email);
            if (string.IsNullOrEmpty(connString))
            {
                ModelState.AddModelError("", "User doesn't not exist.");
            }

            SessionManager.PrivateTenantConnectionString = connString;
            if (SessionManager.CurrentTenant != null && !string.IsNullOrEmpty(SessionManager.CurrentTenant.Components))
            {
                SessionManager.CurrentComponents = SessionManager.CurrentTenant.Components.Split(new char[] { ',' }).ToList();
            }

            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            var db = ApplicationDbContext.Create(connString);

            var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            var signMgr = new ApplicationSignInManager(userMgr, HttpContext.GetOwinContext().Authentication);


            //throw new Exception(connString);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await signMgr.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        var user = userMgr.FindByEmail(model.Email);
                        //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        SessionManager.CurrentUser = user;
                        return RedirectToLocal(returnUrl);
                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "The Email Address or Password provided is incorrect.");
                    return PartialView(model);
            }

        }



        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AdminOnly]
        public ActionResult Add()
        {
            RegisterViewModel model = new RegisterViewModel();

            model.SystemActions = userActionService.GetSystemActions();
            model.SelectedComponents = SessionManager.Components
                .Where(c => SessionManager.CurrentComponents.Contains(c.Id.ToString()))
                .ToList();
            
            return View(model);
        }



        public ActionResult ResetTemporaryPassword()
        {

            return View();
        }
        private void InitIdentityManager()
        {
            var db = ApplicationDbContext.Create(SessionManager.PrivateTenantConnectionString);

            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            var signMgr = new ApplicationSignInManager(UserManager, HttpContext.GetOwinContext().Authentication);

            SignInManager = signMgr;
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AdminOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(RegisterViewModel model)
        {

            model.Password = PasswordGenerator.Generate();
            model.ConfirmPassword = model.Password;

            if (ModelState.IsValid)
            {
                if (TenantHelper.IsExistingUser(model.Email))
                {
                    ModelState.AddModelError("", "Email already exist");
                    return View(model);
                }

                InitIdentityManager();
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Company = model.Company, FirstName = model.Firstname, LastName = model.Lastname, Status = UserStatuses.TemporaryPassword.ToString() };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    TenantHelper.AddEIPUser(model.Email, model.Firstname, model.Lastname, RoleNames.User.ToString());
                    var roleresult = UserManager.AddToRole(user.Id, RoleNames.User);
                    userActionService.UpdateUserPermissions(model.SelectedPermissions, new User { Id = user.Id, Email = user.Email });
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Your Log on Credentials to KUMO Environment", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var rootUrl = Url.Action("Index", "Home", routeValues: null, protocol: Request.Url.Scheme);

                    //add code send email from here.
                    UserMailer.Welcome(user.Email, new WelcomeMailViewModel()
                    {
                        Name = user.FirstName,
                        Password = model.Password,
                        Username = user.Email,
                        Url = rootUrl
                    }).Send();
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ViewBag.Message = "We couldn't find this email address in our records.";
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(model);
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { temp = user.Id, code = code }, protocol: Request.Url.Scheme);

                //add code send email from here.
                UserMailer.PasswordReset(user.Email, new PasswordResetViewModel()
                {
                    Name = user.FirstName,
                    Email = user.Email,
                    ResetUrl = callbackUrl
                }).Send();

                ViewBag.Message = "Please check your email. We have just sent you an email with a link to setup your new password.";
                return RedirectToAction("ForgotPasswordConfirmation", "Account");

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string temp, string code = "")
        {
            if (string.IsNullOrEmpty(temp) || string.IsNullOrEmpty(code))
            {
                return View("Error");
            }

            var user = accountService.GetUserById(temp);
            ResetPasswordViewModel vm = new ResetPasswordViewModel()
            {
                Email = user.Email,
                Code = code
            };

            return code == null || user == null ? View("Error") : View(vm);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var isPasswordValid = await UserManager.CheckPasswordAsync(user, model.Password);
            if (isPasswordValid)
            {
                ViewBag.Message = "The password must be different from your previous password.";
                return View(model);
            }
            //var result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.Password);

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }


        public ActionResult ChangeTemploraryPassword()
        {
            ChangePasswordViewModel vm = new ChangePasswordViewModel()
            {
                Email = User.Identity.Name
            };

            return View(vm);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeTemploraryPassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);

            //var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                UserManager.Update(user);
                user.Status = UserStatuses.Normal.ToString();
                SessionManager.CurrentUser = user;
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SessionManager.CurrentUser = null;


            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        [AdminOnly]
        public ViewResult Index(int page = 1, int pagesize = 20, string orderby = "Id", string keyword = "", string order = "desc")
        {
            ViewBag.SortOrder = (order == "asc" ? "desc" : "asc");
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;

            string orderByExpression = orderby + " " + order;
            var accounts = accountService.GetAllUsers().OrderBy(orderByExpression);

            if (!string.IsNullOrEmpty(keyword))
            {
                accounts = accounts.Where(p => p.Firstname.Contains(keyword) ||
                                                 p.Lastname.Contains(keyword));
            };

            var pageData = accounts.ToPagedList(page, pagesize);
            return View(pageData);

        }


        public JsonResult GetPeoplePaged(int offset, int limit, string search, string sort, string order)
        {
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var data = manager.Users.ToList();
            var total = 10;

            var model = new
            {
                total = total,
                rows = data.ToList(),
            };


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index1()
        {
            return Content(PasswordGenerator.Generate(8));
        }

        [AdminOnly]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = accountService.GetUserById(id);
            var userModel = new UserViewModel();

            if (user == null)
            {
                return HttpNotFound();
            }
            userModel.CopyFrom<UserViewModel>(user);
            userModel.SystemActions = userActionService.GetSystemActions();
            userModel.SelectedComponents = SessionManager.Components
                .Where(c => SessionManager.CurrentComponents.Contains(c.Id.ToString()))
                .ToList();

            userModel.SelectedPermissions = userActionService.GetUserPermissions(user).ToArray();

            var aspNetUser = accountService.GetAspNetUserById(id);
            userModel.IsAdmin = aspNetUser.AspNetRoles.Any(nr => nr.Name == RoleNames.Admin);

            return View(userModel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Company,Phone,SelectedPermissions")] UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.CopyFrom<User>(userModel);

                accountService.UpdateUser(user);
                userActionService.UpdateUserPermissions(userModel.SelectedPermissions, user);
                return RedirectToAction("Index");
            }
            return View(userModel);
        }

        [AdminOnly]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = accountService.GetUserById(id);
            var userModel = new UserViewModel();

            if (user == null)
            {
                return HttpNotFound();
            }
            userModel.CopyFrom<UserViewModel>(user);
            return View(userModel);
        }

        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = accountService.GetUserById(id);
            accountService.DeleteUser(user);

            return RedirectToAction("Index");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}