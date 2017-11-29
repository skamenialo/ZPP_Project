using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ZPP_Project.Models;
using ZPP_Project.Helpers;

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
                ZPP_Project.EntityDataModel.V_Teacher select = DbContext.FindTeacherById(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName;
                    model.LastName = select.LastName;
                    model.Address = select.Address;
                    model.Degree = select.Degree;
                    model.Description = select.Description;
                    model.Website = select.Website;
                }
            }
            else if (ZPPUserRoleHelper.IsStudent(user.UserType))
            {
                ZPP_Project.EntityDataModel.V_Student select = DbContext.FindStudentById(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName;
                    model.LastName = select.LastName;
                    model.Address = select.Address;
                }
            }
            else if (ZPPUserRoleHelper.IsCompany(user.UserType))
            {
                ZPP_Project.EntityDataModel.V_Company select = DbContext.FindCompanyById(user.Id);
                if (select != null)
                {
                    model.Name = select.Name;
                    model.Address = select.Address;
                    model.Email = select.Email;
                    model.Description = select.Description;
                    model.Website = select.Website;
                }
            }
            return View(model);
        }

        public ActionResult PersonalDetails()
        {
            ZppUser user = UserManager.FindByName(User.Identity.Name);
            if (user == null)
                return View("Error");
            if (!(ZPPUserRoleHelper.IsStudent(user.UserType) || ZPPUserRoleHelper.IsTeacher(user.UserType)))
                return RedirectToAction("", "Error");

            PersonalDetailsViewModel model = new PersonalDetailsViewModel()
            {
                IsTeacher = ZPPUserRoleHelper.IsTeacher(user.UserType)
            };
            if (model.IsTeacher)
            {
                ZPP_Project.EntityDataModel.V_Teacher select = DbContext.FindTeacherById(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName.Trim();
                    model.LastName = select.LastName.Trim();
                    model.Address = select.Address.Trim();
                    model.Degree = select.Degree;
                    model.Description = select.Description;
                    model.Website = select.Website;
                }
            }
            else
            {
                ZPP_Project.EntityDataModel.V_Student select = DbContext.FindStudentById(user.Id);
                if (select != null)
                {
                    model.FirstName = select.FirstName.Trim();
                    model.LastName = select.LastName.Trim();
                    model.Address = select.Address.Trim();
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalDetails(PersonalDetailsViewModel model)
        {
            ZppUser user = UserManager.FindByName(User.Identity.Name);
            if (user == null)
                return View("Error");
            if (!(ZPPUserRoleHelper.IsStudent(user.UserType) || ZPPUserRoleHelper.IsTeacher(user.UserType)))
                return RedirectToAction("", "Error");

            if (!ModelState.IsValid)
            {
                model.IsTeacher = ZPPUserRoleHelper.IsTeacher(user.UserType);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult CompanyDetails()
        {
            ZppUser user = UserManager.FindByName(User.Identity.Name);
            if (user == null)
                return View("Error");
            if (!ZPPUserRoleHelper.IsCompany(user.UserType))
                return RedirectToAction("", "Error");

            CompanyDetailsViewModel model = new CompanyDetailsViewModel();

            ZPP_Project.EntityDataModel.V_Company select = DbContext.FindCompanyById(user.Id);
            if (select != null)
            {
                model.Name = select.Name.Trim();
                model.Address = select.Address.Trim();
                model.Email = select.Email.Trim();
                model.Description = select.Description;
                model.Website = select.Website;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDetails(CompanyDetailsViewModel model)
        {
            ZppUser user = UserManager.FindByName(User.Identity.Name);
            if (user == null)
                return View("Error");
            if (!ZPPUserRoleHelper.IsCompany(user.UserType))
                return RedirectToAction("", "Error");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("Index");
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