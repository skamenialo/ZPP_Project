using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZPP_Project.DataAccess;

namespace ZPP_Project.Controllers
{
    public class CompanyController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public CompanyController()
            : base() { }

        public CompanyController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        // GET: Company
        public ActionResult Index()
        {
            return View("Index", DbContext.Companies.ToList());
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Companies.Where(t => t.IdUser == id).FirstOrDefault());
        }
    }
}