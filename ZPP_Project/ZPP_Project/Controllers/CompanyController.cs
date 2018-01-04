using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using PagedList;

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
        public ActionResult Index(int? page, int? pageSize)
        {
            return View("Index", DbContext.Companies.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Companies.Where(company => company.IdCompany == id).FirstOrDefault());
        }
    }
}