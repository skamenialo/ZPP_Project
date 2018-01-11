using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZPP_Project.Models;

namespace ZPP_Project.Helpers
{
    public class SHA512PasswordHasher : PasswordHasher
    {
        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(HashPassword(providedPassword)) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        public override string HashPassword(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] result;
            using (SHA512 shaM = new SHA512Managed())
            {
                result = shaM.ComputeHash(data);
            }
            StringBuilder hex = new StringBuilder(result.Length * 2);
            foreach (byte b in result)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
    }

    public class CookieStoreHelper
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            //HttpCookie encodedCookie = HttpSecureCookie.Encode(new HttpCookie(key, value));
            HttpCookie encodedCookie = new HttpCookie(key, value);

            if (HttpContext.Current.Response.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Response.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Set(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }

        public static void SetCookieAuthorized(string key, string value, TimeSpan expires)
        {
            IOwinContext context = HttpContext.Current.GetOwinContext();
            if (context.Request.User.Identity.IsAuthenticated
                || context.Authentication.AuthenticationResponseGrant.Identity.IsAuthenticated)
            {
                SetCookie(key, value, expires);
            }
            else
            {
                RemoveCookie(key);
            }
        }

        public static string GetCookie(string key)
        {
            string value = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                // For security purpose, we need to encrypt the value.
                //HttpCookie decodedCookie = HttpSecureCookie.Decode(cookie);
                //value = decodedCookie.Value;
                value = cookie.Value;
            }
            return value;
        }

        public static string GetCookieAuthorized(string key)
        {
            if (HttpContext.Current.GetOwinContext().Request.User.Identity.IsAuthenticated)
            {
                return GetCookie(key);
            }
            else
            {
                RemoveCookie(key);
                return null;
            }
        }

        public static void RemoveCookie(string key)
        {
            if (HttpContext.Current.Response.Cookies[key] != null)
            {
                HttpContext.Current.Response.Cookies.Remove(key);
            }
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                HttpContext.Current.Request.Cookies.Remove(key);
            }
        }
    }

    public class ZPPUserRoleHelper
    {
        public static async Task<bool> IsUserInRoleAsync(string roleName, bool supressAdmin = false)
        {
            if (string.IsNullOrEmpty(roleName))
                return false;
            IOwinContext context = HttpContext.Current.GetOwinContext();
            return await IsUserInRoleAsync(context, roleName, context.Authentication.User.Identity.Name, supressAdmin);
        }

        public static async Task<bool> IsUserInRoleAsync(string roleName, string userName, bool supressAdmin = false)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
                return false;
            return await IsUserInRoleAsync(HttpContext.Current.GetOwinContext(), roleName, userName, supressAdmin);
        }

        private static async Task<bool> IsUserInRoleAsync(IOwinContext context, string roleName, string userName, bool supressAdmin = false)
        {
            ApplicationUserManager userManager = context.GetUserManager<ApplicationUserManager>();
            ApplicationRoleManager roleManager = context.Get<ApplicationRoleManager>();

            ZppUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return false;

            ZPPRole role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
                return false;

            if (!await userManager.IsInRoleAsync(user.Id, roleName))
            {
                if (supressAdmin || !await userManager.IsInRoleAsync(user.Id, Roles.ADMINISTRATOR))
                    return false;
            }

            if (HttpContext.Current.Session[Keys.CURRENT_ROLE] == null)
                return false;

            int? roleNr = (int?)HttpContext.Current.Session[Keys.CURRENT_ROLE];
            if (roleNr == null)
                return false;

            List<ZPPUserRole> roles = new List<ZPPUserRole>(user.Roles);

            return roles[roleNr.Value].RoleId == role.Id;
        }

        public static bool IsUserInRole(string roleName, bool supressAdmin = false)
        {
            if (string.IsNullOrEmpty(roleName))
                return false;
            IOwinContext context = HttpContext.Current.GetOwinContext();
            string userName = context.Authentication.User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
                return false;
            return IsUserInRole(context, roleName, userName, supressAdmin);
        }

        public static bool IsUserInRole(string roleName, string userName, bool supressAdmin = false)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
                return false;
            return IsUserInRole(HttpContext.Current.GetOwinContext(), roleName, userName, supressAdmin);
        }

        private static bool IsUserInRole(IOwinContext context, string roleName, string userName, bool supressAdmin = false)
        {
            ApplicationUserManager userManager = context.GetUserManager<ApplicationUserManager>();
            ApplicationRoleManager roleManager = context.Get<ApplicationRoleManager>();

            ZppUser user = userManager.FindByName(userName);
            if (user == null)
                return false;

            ZPPRole role = roleManager.FindByName(roleName);
            if (role == null)
                return false;

            if (!userManager.IsInRole(user.Id, roleName))
            {
                if (supressAdmin || !userManager.IsInRole(user.Id, Roles.ADMINISTRATOR))
                    return false;
            }
            
            if (HttpContext.Current.Session[Keys.CURRENT_ROLE] == null)
                return false;

            int? roleNr = (int?)HttpContext.Current.Session[Keys.CURRENT_ROLE];
            if (roleNr == null)
                return false;

            List<ZPPUserRole> roles = new List<ZPPUserRole>(user.Roles);

            return roles[roleNr.Value].RoleId == role.Id;
        }

        /// <summary>
        /// Checks user for active role ID
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>True role ID or -1 if method failed</returns>
        public static int GetUserRoleNumber(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                return -1;
            int? cookieRoleIndex = (int?)HttpContext.Current.Session[Keys.CURRENT_ROLE];
            if (cookieRoleIndex == null)
                return -1;

            var _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ZppUser user = _userManager.FindByName(userName);
            if (user == null)
                return -1;
            return user.Roles.ElementAt(cookieRoleIndex.Value).RoleId;
        }

        public static string GetUserRoleName(int roleNr)
        {
            switch (roleNr)
            {
                case Roles.ADMINISTRATOR_NR:
                    return Roles.ADMINISTRATOR;
                case Roles.STUDENT_NR:
                    return Roles.STUDENT;
                case Roles.COMPANY_NR:
                    return Roles.COMPANY;
                case Roles.TEACHER_NR:
                    return Roles.TEACHER;
                case Roles.TEACHER_STUDENT_NR:
                    return Roles.TEACHER + " | " + Roles.STUDENT;
                default:
                    return "None";
            }
        }

        #region IsRole methods

        public static bool IsStudent(int? userType)
        {
            return userType == null ? false : IsStudent(userType.Value);
        }

        public static bool IsTeacher(int? userType)
        {
            return userType == null ? false : IsTeacher(userType.Value);
        }

        public static bool IsCompany(int? userType)
        {
            return userType == null ? false : IsCompany(userType.Value);
        }

        public static bool IsAdministrator(int? userType)
        {
            return userType == null ? false : IsAdministrator(userType.Value);
        }

        public static bool IsStudent(int userType)
        {
            return userType == Roles.STUDENT_NR || userType == Roles.TEACHER_STUDENT_NR; //Student || StudentTeacher
        }

        public static bool IsTeacher(int userType)
        {
            return userType > Roles.TEACHER_NR; //Teacher || StudentTeacher
        }

        public static bool IsCompany(int userType)
        {
            return userType == Roles.COMPANY_NR; //Company
        }

        public static bool IsAdministrator(int userType)
        {
            return userType == Roles.ADMINISTRATOR_NR; //Administrator
        }

        public static bool IsRoleValid(int? userType)
        {
            return userType.HasValue && IsRoleValid(userType.Value);
        }

        public static bool IsRoleValid(int userType)
        {
            return userType >= Roles.ADMINISTRATOR_NR && userType <= Roles.TEACHER_STUDENT_NR;
        }

        public static bool IsRoleValid(string userType)
        {
            int uType = 0;
            return Int32.TryParse(userType, out uType) && IsRoleValid(uType);
        }

        public static bool IsRoleValid(string userType, out int type)
        {
            return Int32.TryParse(userType, out type) && IsRoleValid(type);
        }

        #endregion

    }

    /// <summary>
    /// Represents database-specific IDs of various user roles
    /// </summary>
    public static class Roles
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string STUDENT = "Student";
        public const string COMPANY = "Company";
        public const string TEACHER = "Teacher";

        public const int ADMINISTRATOR_NR = 1;
        public const int STUDENT_NR = 2;
        public const int COMPANY_NR = 3;
        public const int TEACHER_NR = 4;
        public const int TEACHER_STUDENT_NR = 5;
    }

    public static class Keys
    {
        public const string CURRENT_ROLE = "CURRENT_ROLE";
        public const string LOGIN_MODEL = "LOGIN_MODEL";
        public const string SELECTED_ROLE = "SELECTED_ROLE";
    }
}