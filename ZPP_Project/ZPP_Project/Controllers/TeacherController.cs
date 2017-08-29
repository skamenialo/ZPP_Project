using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;

namespace ZPP_Project.Controllers
{
    public class TeacherController : Controller
    {
        private ZppContext context = new ZppContext();
        // GET: Teacher
        public ActionResult Index()
        {
            return View("Index", context.Teachers.ToList());
        }

        public ActionResult Details(int id)
        {
            return View("Details", context.Teachers.Where(t => t.IdUser == id).FirstOrDefault());
        }
    }
}