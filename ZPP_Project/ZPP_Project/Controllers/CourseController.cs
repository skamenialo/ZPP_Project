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

        #region Create

        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Create()
        {
            TempData.Remove(Keys.CREATE_COURSE_MODEL);
            TempData.Remove(Keys.CREATE_COURSE_LECTURES);

            CreateCourseViewModel model = new CreateCourseViewModel() { Lectures = 0 };
            if (ZPPUserRoleHelper.IsAdministrator(UserRoleId))
                model.Companies = GetCompanies();
            if (ZPPUserRoleHelper.IsCompany(UserRoleId))
                model.Teachers = GetCompanyTeachers(UserRoleId, true);
            return View(model);
        }

        [HttpPost, ZPPSubmitName("create")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Create(CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = 0;
                V_Company company = null;
                if (ZPPUserRoleHelper.IsCompany(UserRoleId))
                    company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                else
                {
                    if (int.TryParse(model.IdCompany, out id))
                        company = DbContext.Companies.Find(id);
                    else
                        AddError("Parsing error");
                }
                if (company == null)
                    AddError("Company not exist");

                if (DbContext.Courses.FirstOrDefault((c) => c.Name.Equals(model.Name)) != null)
                    AddError("Course with given name already exists.");

                id = 0;
                V_Teacher teacher = null;
                if (string.IsNullOrWhiteSpace(model.IdTeacher) && !model.IdTeacher.Equals("none"))
                {
                    if (int.TryParse(model.IdTeacher, out id))
                    {
                        teacher = DbContext.Teachers.Find(id);
                        if (teacher == null)
                            AddError("Teacher not exist");
                    }
                    else
                        AddError("Parsing error");
                }

                if (ModelState.IsValid)
                {
                    List<V_Lecture> lectures = TempData.Peek(Keys.CREATE_COURSE_LECTURES) as List<V_Lecture>;
                    if (lectures != null)
                        model.Lectures = lectures.Count;
                    else
                        model.Lectures = 0;

                    return CreateCourse(model, lectures);
                }
            }

            if (ZPPUserRoleHelper.IsAdministrator(UserRoleId))
                model.Companies = GetCompanies();
            if (ZPPUserRoleHelper.IsCompany(UserRoleId))
                model.Teachers = GetCompanyTeachers(UserRoleId, true);
            return View(model);
        }

        [HttpPost, ZPPSubmitName("lectures")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult CreateAddLectures(CreateCourseViewModel model)
        {
            TempData[Keys.CREATE_COURSE_MODEL] = model;
            List<V_Lecture> lectures = TempData.Peek(Keys.CREATE_COURSE_LECTURES) as List<V_Lecture>;
            return View("CreateLectures", lectures);
        }

        /// <summary>
        /// The last step in creating course - commiting
        /// This method trust that everything were validated before
        /// </summary>
        private ActionResult CreateCourse(CreateCourseViewModel model, List<V_Lecture> lectures)
        {
            //TODO
            return View("Index");
        }

        #endregion

        private List<SelectListItem> GetCompanies()
        {
            List<SelectListItem> companies = new List<SelectListItem>();
            foreach (V_Company company in DbContext.Companies)
            {
                companies.Add(new SelectListItem()
                {
                    Text = company.Name,
                    Value = company.IdCompany.ToString()
                });
            }
            return companies;
        }

        private IEnumerable<SelectListItem> GetCompanyTeachers(int idCompany, bool addEmpty = false)
        {
            List<SelectListItem> teachers = new List<SelectListItem>();
            if (addEmpty)
                teachers.Add(new SelectListItem() { Text = "None", Value = "none" });
            foreach (V_Teacher teacher in DbContext.Teachers.Where(t => t.IdCompany == idCompany))
            {
                teachers.Add(new SelectListItem()
                {
                    Text = TeacherHelper.Display(teacher),
                    Value = teacher.IdTeacher.ToString()
                });
            }
            return teachers;
        }
    }
}