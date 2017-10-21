using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Controllers
{
    public class ErrorController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public ErrorController()
            : base() { }

        public ErrorController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
    }
}
