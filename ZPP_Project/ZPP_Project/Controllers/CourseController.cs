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
        [Route("Courses/{page?}/{pageSize?}")]
        public ActionResult Index(int? page, int? pageSize)
        {
            if (ZPPUserRoleHelper.IsStudent(this.UserRoleId))
            {
                V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
                List<V_Course> list = new List<V_Course>();
                foreach (var item in DbContext.Courses.Where(c => c.State != 1))
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
            {
                ViewBag.ShowMyCourses = true;
                ViewBag.UserRoleId = this.UserRoleId;
                return View("Index",
                    ZPPUserRoleHelper.IsAdministrator(this.UserRoleId)
                    ? DbContext.Courses.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE)//Admin should see everything
                    : DbContext.Courses.Where(c => c.State != 1).ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE)
                    );
            }
        }

        [Route("Courses/My/{page?}/{pageSize?}")]
        [ZPPAuthorize(Roles = Roles.COMPANY)]
        public ActionResult CompanyCourses(int? page, int? pageSize)
        {
            ViewBag.ShowMyCourses = false;
            return View("Index", DbContext.FindCoursesByCompanyId(DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>()).IdCompany).ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault());
        }

        //GET
        [ZPPAuthorize(Roles = Roles.STUDENT)]
        public ActionResult SignUp(int id)
        {
            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var course = DbContext.Courses.FirstOrDefault(c => c.IdCourse == id);
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

            ViewBag.Header = "Sign up for course";

            return View(model);
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(int id, CourseSignViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");

            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null
                || course.State != 2
                || DbContext.Groups.Any(g => g.IdCourse == course.IdCourse && g.IdStudent == student.IdStudent))
                return View("Error");
            DbContext.Groups.Add(new V_Group()
            {
                IdCourse = course.IdCourse,
                IdStudent = student.IdStudent
            });
            DbContext.SaveChanges();

            return View("GenericMessage", new GenericViewModel()
            {
                Title = "Signed up",
                Message = "You signed in to course " + model.Name,
                ButtonText = "Back to Courses",
                ButtonHref = @"/Courses/"
            });
        }

        //GET
        [ZPPAuthorize(Roles = Roles.STUDENT)]
        public ActionResult SignOut(int id)
        {
            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var course = DbContext.Courses.FirstOrDefault(c => c.IdCourse == id);
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

            ViewBag.Header = "Sign out of course";

            return View("SignUp", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut(int id, CourseSignViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");

            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null
                || course.State != 2)
                return View("Error");
            var group = DbContext.Groups.FirstOrDefault(g => g.IdCourse == course.IdCourse && g.IdStudent == student.IdStudent);
            if (group == null)
                return View("Error");
            DbContext.Groups.Remove(group);
            DbContext.SaveChanges();

            return View("GenericMessage", new GenericViewModel()
            {
                Title = "Signed out",
                Message = "You signed out from course " + model.Name,
                ButtonText = "Back to Courses",
                ButtonHref = @"/Courses/"
            });
        }
    }
}