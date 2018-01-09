using System;
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
using ZPP_Project.DataAccess;

namespace ZPP_Project.Helpers
{
    public class ZPPController : Controller
    {
        #region Fields

        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";
        protected int UserRoleId;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ZppContext _context;

        #endregion

        #region Properties

        public ApplicationSignInManager SignInManager
        {
            get
            {
                if (_signInManager == null)
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                return _signInManager;
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
                if (_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
                return _roleManager;
            }
            private set { _roleManager = value; }
        }

        public ZppContext DbContext
        {
            get
            {
                if (_context == null)
                    _context = ZppContext.Create();
                return _context;
            }
            private set { _context = value; }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion

        #region Constructor

        public ZPPController()
            : base() { }

        public ZPPController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : this()
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        #endregion

        #region IDisposable members

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

        #endregion

        #region Controller members

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //przywracanie sesji
            if (User.Identity.IsAuthenticated)
                try
                {
                    RestoreSession();
                }
                catch (NullReferenceException)
                {
                    ClearSession();
                }
                catch(Exception)
                {

                }
            else
                ClearSession();
            this.UserRoleId = ZPP_Project.Helpers.ZPPUserRoleHelper.GetUserRoleNumber(User.Identity.Name);
            ViewBag.UserRoleId = this.UserRoleId;
        }

        #endregion

        protected void RestoreSession()
        {
            if (Session[Helpers.Keys.CURRENT_ROLE] == null)
            {
                string role = Helpers.CookieStoreHelper.GetCookieAuthorized(Helpers.Keys.CURRENT_ROLE);
                if (!string.IsNullOrEmpty(role))
                {
                    int roleNr = 0;
                    Int32.TryParse(role, out roleNr);
                    if (roleNr < 0)
                        roleNr = 0;
                    SetUserRole(User.Identity.Name, roleNr);
                }
            }
        }

        protected void ClearSession()
        {
            Session.Remove(Helpers.Keys.CURRENT_ROLE);
            Helpers.CookieStoreHelper.RemoveCookie(Helpers.Keys.CURRENT_ROLE);
        }

        protected void SetUserRole(string userName, int roleNr)
        {
            ZppUser user = UserManager.FindByName(userName);
            SetUserRole(user, roleNr);
        }

        protected async Task SetUserRoleAsync(string userName, int roleNr)
        {
            ZppUser user = await UserManager.FindByNameAsync(userName);
            SetUserRole(user, roleNr);
        }

        private void SetUserRole(ZppUser user, int roleNr)
        {
            int rolesCount = user.Roles.Count();
            if (rolesCount > 0)
            {
                if (roleNr >= rolesCount || roleNr < 0)
                    roleNr = 0;
                Session[Helpers.Keys.CURRENT_ROLE] = roleNr;
                Helpers.CookieStoreHelper.SetCookieAuthorized(Helpers.Keys.CURRENT_ROLE, roleNr.ToString(), TimeSpan.FromMinutes(60));
            }
        }

        protected void AddError(string error)
        {
            ModelState.AddModelError("", error);
        }

        protected void AddError<TModel, TProperty>(TModel model, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, string error)
        {
            ModelState.AddModelError(Helpers.ZPPHtmlExtensions.GetDisplayName<TModel, TProperty>(model, expression), error);
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected async Task<ActionResult> GenerateEmailConfirmation(int userId)
        {
            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: HttpContext.Request.Url.Scheme);
            await UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

#if DEBUG
            ViewBag.Link = callbackUrl;
            return View("DisplayEmail");
#else
            return View("Login");
#endif
        }
    }
}