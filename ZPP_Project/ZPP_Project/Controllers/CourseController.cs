using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using ZPP_Project.Models;
using PagedList;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;

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
        public ActionResult Index(int? page, int? pageSize)
        {
            if (this.UserRoleId == (int)Helpers.Role.Student)
            {
                V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
                List<V_Course> list = new List<V_Course>();
                foreach (var item in DbContext.Courses.ToList())
                {
                    if (DbContext.Groups.Any(g => g.IdCourse == item.IdCourse && g.IdStudent == student.IdStudent))
                    {
                        list.Add(new V_CourseExtended(item) { IdStudent = student.IdStudent, IsMember = true });
                    }
                    else
                        list.Add(item);
                }
                return View("Index", list.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
            }
            else
                return View("Index", DbContext.Courses.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault());
        }

        //GET
        [ZPPAuthorize(Roles = Roles.STUDENT)]
        public ActionResult SignUp(int courseId)
        {
            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var course = DbContext.Courses.FirstOrDefault(c => c.IdCourse == courseId);
            if (course == null)
                return View("Error");

            var teacher = DbContext.Teachers.FirstOrDefault(t => t.IdTeacher == course.IdTeacher);
            var company = DbContext.Companies.FirstOrDefault(c => c.IdCompany == course.IdCompany);

            var model = new CourseSignViewModel()
            {
                IdStudent = student.IdStudent,
                IdCourse = course.IdCourse,
                Name = course.Name,
                DateEnd = course.DateEnd,
                DateStart = course.DateStart,
                Description = course.Description,
                Lectures = course.Lectures,
                TeacherFullName = teacher == null ? ProgramData.VALUE_UNKNOWN : TeacherHelper.Display(teacher),
                CompanyFullName = company == null ? ProgramData.VALUE_UNKNOWN : CompanyHelper.Display(company),
            };

            return View(model);
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(CourseSignViewModel model)
        {
            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());

            throw new NotImplementedException();
        }

        //GET
        [ZPPAuthorize(Roles = Roles.STUDENT)]
        public ActionResult SignOut(int courseId)
        {
            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var model = new CourseSignViewModel();

            throw new NotImplementedException();
        }
    }
}