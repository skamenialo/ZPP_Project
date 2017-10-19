using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZPP_Project.DataAccess;

namespace ZPP_Project.Controllers
{
    public class CompanyController : Controller
    {
        private ZppContext context = new ZppContext();
        // GET: Company
        public ActionResult Index()
        {
            return View("Index", context.Companies.ToList());
        }

        public ActionResult Details(int id)
        {
            return View("Details", context.Companies.Where(t => t.IdUser == id).FirstOrDefault());
        }
    }
}