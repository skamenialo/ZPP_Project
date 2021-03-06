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
using ZPP_Project.Models;
using System.Collections.Generic;
using ZPP_Project.Helpers;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Controllers
{
    [Authorize]
    public class AccountController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public AccountController()
            : base() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        #region Login

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData.Remove(Keys.LOGIN_MODEL);
            try
            {
                // Verification.    
                if (this.Request.IsAuthenticated)
                {
                    // Info.    
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            ViewBag.ReturnUrl = returnUrl;
            return this.View();    
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                TempData.Remove(Keys.LOGIN_MODEL);
                return View(model);
            }

            ZppUser user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null && UserManager.CheckPassword(user, model.Password))
            {
                if (user.Roles.Count > 1)
                {
                    List<SelectListItem> roles = new List<SelectListItem>();
                    for (int i = 0; i < user.Roles.Count; i++)
                    {
                        ZPPUserRole ur = user.Roles.ElementAt(i);
                        roles.AddRange(RoleManager.Roles.Where(role => role.Id == ur.RoleId).Select(r => new SelectListItem
                        {
                            Text = r.Name,
                            Value = i.ToString()
                        }));
                    }
                    TempData[Keys.LOGIN_MODEL] = model;
                    return View("SelectRole", new SelectRoleViewModel() { SelectedId = "-1", Roles = roles });
                }
                return await SignIn(model, 0, returnUrl);
            }
            else
            {
                AddError("Invalid login attempt.");
                ViewBag.ReturnUrl = returnUrl;
                return View("Login", model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginWithRole(SelectRoleViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            LoginViewModel loginModel = TempData[Keys.LOGIN_MODEL] as LoginViewModel;
            if (loginModel == null)
            {
                AddError("Invalid login attempt.");
                ViewBag.ReturnUrl = returnUrl;
                return View("Login");
            }

            int roleNr = 0;
            Int32.TryParse(model.SelectedId, out roleNr);
            return await SignIn(loginModel, roleNr, returnUrl);
        }

        [AllowAnonymous]
        public async Task<ActionResult> LoginWithDetails(string returnUrl)
        {
            LoginViewModel loginModel = TempData[Keys.LOGIN_MODEL] as LoginViewModel;
            int roleNr = (int)TempData[Keys.SELECTED_ROLE];
            return await SignIn(loginModel, roleNr, returnUrl);
        }

        private async Task<ActionResult> SignIn(LoginViewModel model, int roleNr, string returnUrl)
        {
            if (!await UserManager.IsEmailConfirmedAsync(model.UserName))
            {
                if (UserManager.FindByName(model.UserName) != null)
                {
                    string callbackUrl = Url.Action("GenerateEmailConfirmation", "Account", new { userName = model.UserName }, protocol: Request.Url.Scheme);
                    AddError("You need to confirm your email. Click <a href=\"" + callbackUrl + "\">here</a> to generate new one");
                }
                else
                    AddError("Invalid login attempt.");
            }
            else
            {
                ZppUser user = UserManager.FindByName(model.UserName);
                if (user != null)
                {
                    //details not needed for Administrator
                    bool detailsNeeded = ZPPUserRoleHelper.IsAdministrator(user.UserType);
                    if (!detailsNeeded)
                    {
                        if (ZPPUserRoleHelper.IsTeacher(user.UserType))
                        {
                            V_Teacher select = DbContext.FindTeacherByUserId(user.Id);
                            detailsNeeded = select == null || string.IsNullOrWhiteSpace(select.FirstName);
                        }
                        else if (ZPPUserRoleHelper.IsStudent(user.UserType))
                        {
                            V_Student select = DbContext.FindStudentByUserId(user.Id);
                            detailsNeeded = select == null || string.IsNullOrWhiteSpace(select.FirstName);
                        }
                        else if (ZPPUserRoleHelper.IsCompany(user.UserType))
                        {
                            V_Company select = DbContext.FindCompanyByUserId(user.Id);
                            detailsNeeded = select == null || string.IsNullOrWhiteSpace(select.Name);
                        }
                        if (detailsNeeded)
                        {
                            TempData[Keys.LOGIN_MODEL] = model;
                            TempData[Keys.SELECTED_ROLE] = roleNr;
                            if (ZPPUserRoleHelper.IsCompany(user.UserType))
                                return RedirectToAction("CompanyDetails", "Manage");
                            else
                                return RedirectToAction("PersonalDetails", "Manage");
                        }
                    }
                }
                else
                    AddError("Invalid login attempt.");
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                SignInStatus result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:
                        await SetUserRoleAsync(model.UserName, roleNr);
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        AddError("Invalid login attempt.");
                        break;
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (UserManager.FindByName(model.UserName) == null)
                    if (UserManager.FindByEmail(model.Email) == null)
                    {
                        ZppUser user = new ZppUser { UserName = model.UserName, Email = model.Email, UserType = Roles.STUDENT_NR };

                        var res = await UserManager.UserValidator.ValidateAsync(user);
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            DbContext.Entry(new V_StudentData()
                            {
                                IdUser = user.Id,
                                LastName = " ",
                                FirstName = " ",
                                Address = " "
                            }).State = System.Data.Entity.EntityState.Added;
                            DbContext.SaveChanges();
                            //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false
                            //return RedirectToAction("Index", "Home");

                            return await GenerateEmailConfirmation(user.Id);
                        }
                        else
                            AddErrors(result);
                    }
                    else
                        AddError("Email already taken");
                else
                    AddError("User name already taken");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/GenerateEmailConfirmation
        [AllowAnonymous]
        public async Task<ActionResult> GenerateEmailConfirmation(string userName)
        {
            ZppUser user = await UserManager.FindByNameAsync(userName);
            if (user != null)
                return await GenerateEmailConfirmation(user.Id);
            else
                return View("Error");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            int id = 0;
            if (!int.TryParse(userId, out id) || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(id, code);
            if (result.Succeeded)
            {
                ZppUser user = await UserManager.FindByIdAsync(id);
                switch (user.UserType)
                {
                    case Roles.STUDENT_NR:
                        result = await UserManager.AddToRoleAsync(id, Roles.STUDENT);
                        break;
                    case Roles.COMPANY_NR:
                        result = await UserManager.AddToRoleAsync(id, Roles.COMPANY);
                        break;
                    case Roles.TEACHER_NR:
                        result = await UserManager.AddToRoleAsync(id, Roles.TEACHER);
                        break;
                    case Roles.TEACHER_STUDENT_NR:
                        result = await UserManager.AddToRoleAsync(id, Roles.STUDENT);
                        result = await UserManager.AddToRoleAsync(id, Roles.TEACHER);
                        break;
                    default:
                        break;
                }
                return View("ConfirmEmail");
            }
            return View("Error");
        }

        #endregion

        #region Password

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
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { user = user.Email, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

#if DEBUG
                ViewBag.Link = callbackUrl;
                return View("DisplayEmail");
#else
                return View("ForgotPasswordConfirmation");
#endif
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
        public ActionResult ResetPassword(string user, string code)
        {
            return code == null ? View("Error") : View(new ResetPasswordViewModel() { Email = user});
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
            if (model.Password.Equals(model.ConfirmPassword))
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
            }
            else
                AddError("Confirmation password does not match.");
            model.ConfirmPassword = "";
            model.Password = "";
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region VerifyCode

        //
        // GET: /Account/VerifyCode
        //[AllowAnonymous]
        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // Require that the user has already logged in via username/password or external login
        //    if (!await SignInManager.HasBeenVerifiedAsync())
        //    {
        //        return View("Error");
        //    }
        //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        //
        // POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            AddError("Invalid code.");
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/SendCode
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId < 1)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        //
        // POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    // Generate the token and send it
        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return View("Error");
        //    }
        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //}

        #endregion
        
        #region ExternalLogin

        //
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ZppUser { UserName = model.UserName, Email = model.Email, UserType = 2};
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        #endregion

        #region Helpers

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