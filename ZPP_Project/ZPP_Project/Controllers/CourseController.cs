using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using ZPP_Project.Models;

namespace ZPP_Project.Controllers
{
    public class CourseController : Controller
    {
        private ZppContext context = new ZppContext();
        // GET: Course
        public ActionResult Index()
        {
            return View("Index", context.Courses.ToList());
        }
    }
}