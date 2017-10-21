﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZPP_Project.Models;
using System.Collections.Generic;

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
                //HttpContext.Current.Response.Cookies.Add(cookieOld);
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
        public static async Task<bool> IsUserInRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return false;
            IOwinContext context = HttpContext.Current.GetOwinContext();
            return await IsUserInRoleAsync(context, roleName, context.Authentication.User.Identity.Name);
        }

        public static async Task<bool> IsUserInRoleAsync(string roleName, string userName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
                return false;
            return await IsUserInRoleAsync(HttpContext.Current.GetOwinContext(), roleName, userName);
        }

        private static async Task<bool> IsUserInRoleAsync(IOwinContext context, string roleName, string userName)
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
                return false;

            if (HttpContext.Current.Session[Keys.CURRENT_ROLE] == null)
                return false;

            string r = (string)HttpContext.Current.Session[Keys.CURRENT_ROLE];
            if (string.IsNullOrEmpty(r))
                return false;

            int roleNr = 0;
            Int32.TryParse(r, out roleNr);

            List<ZPPUserRole> roles = new List<ZPPUserRole>(user.Roles);

            return roles[roleNr].RoleId == role.Id;
        }

        public static bool IsUserInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return false;
            IOwinContext context = HttpContext.Current.GetOwinContext();
            return IsUserInRole(context, roleName, context.Authentication.User.Identity.Name);
        }

        public static bool IsUserInRole(string roleName, string userName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
                return false;
            return IsUserInRole(HttpContext.Current.GetOwinContext(), roleName, userName);
        }

        private static bool IsUserInRole(IOwinContext context, string roleName, string userName)
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
                return false;
            
            if (HttpContext.Current.Session[Keys.CURRENT_ROLE] == null)
                return false;

            int roleNr = (int)HttpContext.Current.Session[Keys.CURRENT_ROLE];
            List<ZPPUserRole> roles = new List<ZPPUserRole>(user.Roles);

            return roles[roleNr].RoleId == role.Id;
        }
    }

    public static class Roles
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string STUDENT = "Student";
        public const string COMPANY = "Firma";
        public const string TEACHER = "Wykladowca";
    }

    public static class Keys
    {
        public const string CURRENT_ROLE = "CURRENT_ROLE";
        public const string LOGIN_MODEL = "LOGIN_MODEL";
    }
}