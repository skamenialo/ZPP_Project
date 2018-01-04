using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using PagedList;

namespace ZPP_Project.Controllers
{
    public class TeacherController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public TeacherController()
            : base() { }

        public TeacherController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        // GET: Teacher
        public ActionResult Index(int? page, int? pageSize)
        {
            return View("Index", DbContext.Teachers.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Teachers.Where(teacher => teacher.IdTeacher == id).FirstOrDefault());
        }
    }
}