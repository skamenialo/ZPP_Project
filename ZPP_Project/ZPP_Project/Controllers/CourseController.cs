using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using ZPP_Project.Models;

namespace ZPP_Project.Controllers
{
    public class CourseController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public CourseController()
            : base() { }

        public CourseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        // GET: Course
        public ActionResult Index()
        {
            return View("Index", DbContext.Courses.ToList());
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault());
        }
    }
}