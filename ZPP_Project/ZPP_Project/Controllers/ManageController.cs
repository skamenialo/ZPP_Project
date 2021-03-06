﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ZPP_Project.Models;
using ZPP_Project.Helpers;
using ZPP_Project.EntityDataModel;
using System.Collections.Generic;
using System.Data.Entity;

namespace ZPP_Project.Controllers
{
    [ZPPAuthorize]
    public class ManageController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public ManageController()
            : base() { }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            ZppUser user =  UserManager.FindByName(User.Identity.Name);
            var model = new IndexViewModel
            {
                UserType = user.UserType,
                HasPassword = await UserManager.HasPasswordAsync(user.Id),
                //PhoneNumber = await UserManager.GetPhoneNumberAsync(IntUserId),
                //TwoFactor = await UserManager.GetTwoFactorEnabledAsync(IntUserId),
                //Logins = await UserManager.GetLoginsAsync(IntUserId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(user.Id.ToString())
            };
            if (ZPPUserRoleHelper.IsTeacher(user.UserType))
            {
                V_Teacher select = DbContext.FindTeacherByUserId(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName;
                    model.LastName = select.LastName;
                    model.Address = select.Address;
                    model.Degree = select.Degree;
                    model.Description = select.Description;
                    model.Website = select.Website;
                    model.IdCompany = select.IdCompany;
                }
            }
            else if (ZPPUserRoleHelper.IsStudent(user.UserType))
            {
                V_Student select = DbContext.FindStudentByUserId(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName;
                    model.LastName = select.LastName;
                    model.Address = select.Address;
                }
            }
            else if (ZPPUserRoleHelper.IsCompany(user.UserType))
            {
                V_Company select = DbContext.FindCompanyByUserId(user.Id);
                if (select != null)
                {
                    model.Name = select.Name;
                    model.Address = select.Address;
                    model.Email = select.EmailCompany;
                    model.Description = select.Description;
                    model.Website = select.Website;
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult PersonalDetails()
        {
            ZppUser user = TryGetZppUser();
            if (user == null || !(ZPPUserRoleHelper.IsStudent(user.UserType) || ZPPUserRoleHelper.IsTeacher(user.UserType)))
                return RedirectToAction("", "Error");

            PersonalDetailsViewModel model = new PersonalDetailsViewModel()
            {
                IsTeacher = ZPPUserRoleHelper.IsTeacher(user.UserType)
            };
            if (model.IsTeacher)
            {
                V_Teacher select = DbContext.FindTeacherByUserId(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName.Trim();
                    model.LastName = select.LastName.Trim();
                    model.Address = select.Address.Trim();
                    model.Degree = select.Degree;
                    model.Description = select.Description;
                    model.Website = select.Website;
                }
                else
                    return View("Error");
            }
            else
            {
                V_Student select = DbContext.FindStudentByUserId(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName.Trim();
                    model.LastName = select.LastName.Trim();
                    model.Address = select.Address.Trim();
                }
                else
                    return View("Error");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult PersonalDetails(PersonalDetailsViewModel model)
        {
            bool login;
            ZppUser user = TryGetZppUser(out login);
            if (user == null || !(ZPPUserRoleHelper.IsStudent(user.UserType) || ZPPUserRoleHelper.IsTeacher(user.UserType)))
                return RedirectToAction("", "Error");

            if (!ModelState.IsValid)
            {
                model.IsTeacher = ZPPUserRoleHelper.IsTeacher(user.UserType);
                return View(model);
            }

            if (ZPPUserRoleHelper.IsTeacher(user.UserType))
            {
                V_Teacher select = DbContext.FindTeacherByUserId(user.Id);
                if (select != null)
                {
                    DbContext.Entry(new V_TeacherInfo()
                    {
                        IdTeacher = select.IdTeacher,
                        IdCompany = select.IdCompany,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        Degree = model.Degree,
                        Website = model.Website,
                        Description = model.Description
                    }).State = EntityState.Modified;
                    DbContext.SaveChanges();
                }
                else
                    return RedirectToAction("", "Error");
            }

            if (ZPPUserRoleHelper.IsStudent(user.UserType))
            {
                V_Student select = DbContext.FindStudentByUserId(user.Id);
                if (select != null)
                {
                    DbContext.Entry(new V_StudentInfo()
                    {
                        IdStudent = select.IdStudent,
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        Address = model.Address.Trim()
                    }).State = EntityState.Modified;
                    DbContext.SaveChanges();
                }
                else
                    return RedirectToAction("", "Error");
            }

            if (login)
                return RedirectToAction("LoginWithDetails", "Account");
            else
                return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult CompanyDetails()
        {
            ZppUser user = TryGetZppUser();
            if (user == null || !ZPPUserRoleHelper.IsCompany(user.UserType))
                return RedirectToAction("", "Error");

            V_Company select = DbContext.FindCompanyByUserId(user.Id);
            if (select != null)
            {
                CompanyDetailsViewModel model = new CompanyDetailsViewModel()
                {
                    Name = select.Name.Trim(),
                    Address = select.Address.Trim(),
                    Email = select.EmailCompany.Trim(),
                    Description = select.Description,
                    Website = select.Website
                };
                return View(model);
            }
            else
                return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CompanyDetails(CompanyDetailsViewModel model)
        {
            bool login;
            ZppUser user = TryGetZppUser(out login);
            if (user == null || !ZPPUserRoleHelper.IsCompany(user.UserType))
                return RedirectToAction("", "Error");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            V_Company select = DbContext.FindCompanyByUserId(user.Id);
            if (select != null)
            {
                DbContext.Entry(new V_CompanyInfo()
                {
                    IdCompany = select.IdCompany,
                    Email = select.EmailCompany,
                    Name = model.Name,
                    Address = model.Address,
                    Website = model.Website,
                    Description = model.Description
                }).State = EntityState.Modified;
                DbContext.SaveChanges();
                if (login)
                    return RedirectToAction("LoginWithDetails", "Account");
                else
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("", "Error");

        }

        private ZppUser TryGetZppUser()
        {
            bool login;
            return TryGetZppUser(out login);
        }

        private ZppUser TryGetZppUser(out bool login)
        {
            LoginViewModel loginModel = TempData.Peek(Keys.LOGIN_MODEL) as LoginViewModel;
            login = loginModel != null;
            if (login)
                return UserManager.FindByName(loginModel.UserName);
            else
                return UserManager.FindByName(User.Identity.Name);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            AddError("Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId<int>(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

#region Helpers

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}