using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZPP_Project.Controllers
{
    public class HomeController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public HomeController()
            : base() { }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}