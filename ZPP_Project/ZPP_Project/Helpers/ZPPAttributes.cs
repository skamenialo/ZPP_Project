using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZPP_Project.Helpers
{
    public class ZPPAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] RolesArray
        {
            get { return Roles.Split(','); }
            set { Roles = string.Join(",", value); }
        }

        //
        // Summary:
        //     Processes HTTP requests that fail authorization.
        //
        // Parameters:
        //   filterContext:
        //     Encapsulates the information for using System.Web.Mvc.AuthorizeAttribute.
        //     The filterContext object contains the controller, HTTP context, request context,
        //     action result, and route data.
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Error");
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ZPPSubmitNameAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }

        public ZPPSubmitNameAttribute(string name)
        {
            Name = name;
        }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[Name] != null;
        }
    }
}