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
            if (this.UserRoleId != -1)
                ViewBag.ShowMyCourses = true;
            if (ZPPUserRoleHelper.IsStudent(this.UserRoleId))
            {
                V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
                List<V_Course> list = new List<V_Course>();
                var grades = DbContext.Grades.Where(g => g.IdStudent == student.IdStudent).ToList();
                foreach (var item in DbContext.Courses.Where(c => c.State != 1))
                {
                    if (DbContext.Groups.Any(g => g.IdCourse == item.IdCourse && g.IdStudent == student.IdStudent))
                    {
                        var grade = grades.FirstOrDefault(g => g.IdCourse == item.IdCourse);
                        list.Add(new V_CourseExtended(item)
                        {
                            IdStudent = student.IdStudent,
                            Grade = grade != null
                                ? (double?)grade.Grade
                                : null,
                            IsMember = true
                        });
                    }
                    else
                        list.Add(item);
                }
                return View("Index", list.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
            }
            else if (ZPPUserRoleHelper.IsTeacher(this.UserRoleId))
            {
                V_Teacher teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());
                List<V_Course> list = new List<V_Course>();

                foreach (var item in DbContext.Courses)
                {
                    if (teacher.IdTeacher == item.IdTeacher)
                    {
                        //extended
                        list.Add(new V_CourseExtended(item)
                        {
                            IdTeacher = teacher.IdTeacher,
                            IsMember = true
                        });
                    }
                    else if (item.State != 1)
                        list.Add(item);
                }
                return View("Index", list.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
            }
            else
            {
                ViewBag.UserRoleId = this.UserRoleId;
                return View("Index",
                    ZPPUserRoleHelper.IsAdministrator(this.UserRoleId)
                    ? DbContext.Courses.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE)//Admin should see everything
                    : DbContext.Courses.Where(c => c.State != 1).ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE)
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
            if (ZPPUserRoleHelper.IsStudent(this.UserRoleId))
            {
                V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
                V_Course course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();

                var grade = DbContext.Grades.Where(g => g.IdStudent == student.IdStudent && g.IdCourse == course.IdCourse).FirstOrDefault();
                var model = new V_CourseExtended(course)
                {
                    IdStudent = student.IdStudent,
                    Grade = grade == null ? null  : (double?)grade.Grade,
                    IsMember = DbContext.Groups.Any(g => g.IdCourse == course.IdCourse && g.IdStudent == student.IdStudent)
                };
                return View("Details", model);
            }
            else if (ZPPUserRoleHelper.IsTeacher(this.UserRoleId))
            {
                V_Teacher teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());
                V_Course course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
                if (course.IdTeacher == teacher.IdTeacher)
                {
                    var model = new V_CourseExtended(course)
                    {
                        IdTeacher = teacher.IdTeacher,
                        IsMember = true,
                    };
                    return View("Details", model);
                }

            }

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

        [ZPPAuthorize]
        public ActionResult Attendance(int id)
        {
            V_Student student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            var lectures = new List<V_Lecture>(DbContext.Lectures.Where(l => l.IdCourse == course.IdCourse));
            var lectureIds = lectures.Select(l => l.IdLecture);
            var attendanceModel = new AttendanceViewModel()
            {
                CourseName = course.Name,
                StudentName = ZPP_Project.Helpers.StudentHelper.Display(student),
                Lectures = DbContext.Attendance.Where(a => a.IdStudent == student.IdStudent && lectureIds.Contains(a.IdLecture) == true).ToList().Select(a => new LectureAttendanceViewModel()
                {
                    Attended = a.Attended,
                    IdAttendance = a.IdAttendance,
                    LecuteDate = lectures.FirstOrDefault(l => l.IdLecture == a.IdLecture).LecuteDate
                }).ToList()
            };

            return View(attendanceModel);
        }

        [ZPPAuthorize(RolesArray = new[] {Roles.STUDENT, Roles.ADMINISTRATOR})]
        public ActionResult Student(int? id, int? page, int? pageSize)
        {
            V_Student student = null;
            if (ZPPUserRoleHelper.IsAdministrator(this.UserRoleId))
                student = DbContext.FindStudentByUserId(id ?? -1);
            else
                student = DbContext.FindStudentByUserId(User.Identity.GetUserId<int>());

            if (student != null)
            {
                ViewBag.ShowMyCourses = false;
                var groups = DbContext.Groups.Where(g => g.IdStudent == student.IdStudent).Select(g => g.IdCourse).ToList();
                var courses = DbContext.Courses.Where(c => groups.Contains(c.IdCourse)).ToList();
                var model = new List<V_CourseExtended>();
                foreach (var c in courses)
                {
                    model.Add(new V_CourseExtended(c)
                    {
                        IdStudent = student.IdStudent,
                        Grade = (double?)(DbContext.Grades.Where(g => g.IdStudent == student.IdStudent && g.IdCourse == c.IdCourse).FirstOrDefault() ?? new V_Grade()).Grade,
                        IsMember = true,
                    });
                }

                return View("Index", model.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
            }
            else
                return View("Error");
        }

        [ZPPAuthorize(RolesArray = new[] { Roles.TEACHER, Roles.ADMINISTRATOR })]
        public ActionResult Teacher(int? id, int? page, int? pageSize)
        {
            V_Teacher teacher = null;
            if (ZPPUserRoleHelper.IsAdministrator(this.UserRoleId))
                teacher = DbContext.FindTeacherByUserId(id ?? -1);
            else
                teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());

            if (teacher != null)
            {
                ViewBag.ShowMyCourses = false;
                var courses = DbContext.Courses.Where(c => c.IdTeacher == teacher.IdTeacher).ToList();
                var model = new List<V_CourseExtended>();
                foreach (var c in courses)
                {
                    model.Add(new V_CourseExtended(c)
                    {
                        IdTeacher = teacher.IdTeacher,
                    });
                }
                return View("Index", model.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
            }
            else
                return View("Error");
        }

        [ZPPAuthorize(RolesArray = new[] { Roles.TEACHER, Roles.COMPANY, Roles.ADMINISTRATOR })]
        public ActionResult GradeEdit(int id)
        {
            //authorization
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null)
                return View("Error");
            if (ZPPUserRoleHelper.IsCompany(this.UserRoleId))
            {
                var company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                if (company == null || company.IdCompany != course.IdCourse)
                    return View("Error");
            }
            else if (ZPPUserRoleHelper.IsTeacher(this.UserRoleId))
            {
                var teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());
                if (teacher == null || teacher.IdTeacher != course.IdCourse)
                    return View("Error");
            }
            //authorisation check ok, display stuff
            var model = new GradeEditViewModel();
            model.CourseName = course.Name;
            model.Items.AddRange(DbContext.Grades.Where(g => g.IdCourse == course.IdCourse).Select(g => new GradeEditItemViewModel()
                {
                    IdGrade = g.IdGrade,
                    IdStudent = g.IdStudent,
                    IdCourse = g.IdCourse,
                    Grade = g.Grade,
                    Date = g.Date,
                    IdTeacher = g.IdTeacher,
                    Comment = g.Comment,
                }));
            List<int> ids = model.Items.Select(item => item.IdStudent).ToList();
            model.Items.AddRange(DbContext.Groups.Where(g => g.IdCourse == course.IdCourse && !ids.Contains(g.IdStudent)).Select(g => new GradeEditItemViewModel()
                {
                    IdStudent = g.IdStudent,
                    IdCourse = course.IdCourse,
                }));
            foreach (var item in model.Items)
            {
                item.StudentName = ZPP_Project.Helpers.StudentHelper.Display(item.IdStudent);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.TEACHER, Roles.COMPANY, Roles.ADMINISTRATOR })]
        public ActionResult GradeEdit(int id, GradeEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");

            //authorization
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null)
                return View("Error");
            if (ZPPUserRoleHelper.IsCompany(this.UserRoleId))
            {
                var company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                if (company == null || company.IdCompany != course.IdCourse)
                    return View("Error");
            }
            else if (ZPPUserRoleHelper.IsTeacher(this.UserRoleId))
            {
                var teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());
                if (teacher == null || teacher.IdTeacher != course.IdCourse)
                    return View("Error");
            }
            //authorisation check ok, parse results
            var groups = DbContext.Groups.Where(g => g.IdCourse == course.IdCourse).ToList();
            //warning: each item must be checked again!
            foreach (var item in model.Items)
            {
                if (item.IdGrade != 0)//grade updated
                {
                    var grade = DbContext.Grades.Where(g => g.IdGrade == item.IdGrade).FirstOrDefault();
                    if (grade == null || grade.IdStudent != item.IdStudent//wrong student, reject
                        || ((item.Grade.HasValue && item.Grade.Value == grade.Grade)//nothing changed, reject
                            && item.Comment.Equals(grade.Comment)))
                        continue;
                    if (item.Grade.HasValue)
                        grade.Grade = item.Grade.Value;
                    grade.Comment = item.Comment;
                    grade.Date = DateTime.Now;
                }
                else//new grade
                {
                    if (item.IdStudent == null || !item.Grade.HasValue)
                        continue;
                    var newGrade = new V_Grade()
                    {
                        IdStudent = item.IdStudent,
                        IdTeacher = item.IdTeacher,
                        IdCourse = course.IdCourse,
                        Grade = item.Grade.Value,
                        Comment = item.Comment,
                        Date = DateTime.Now,
                    };
                    DbContext.Grades.Add(newGrade);
                }
            }

            DbContext.SaveChanges();
            return View("GenericMessage", new GenericViewModel()
            {
                Title = "Changes saved",
                Message = "Grades for course \"" + course.Name + "\" altered",
                ButtonText = "Back to Courses",
                ButtonHref = @"/Courses"
            });
        }

        [ZPPAuthorize(RolesArray = new[] { Roles.TEACHER, Roles.COMPANY, Roles.ADMINISTRATOR })]
        public ActionResult AttendanceEdit(int id)
        {
            //authorization
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null)
                return View("Error");
            if (ZPPUserRoleHelper.IsCompany(this.UserRoleId))
            {
                var company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                if (company == null || company.IdCompany != course.IdCourse)
                    return View("Error");
            }
            else if (ZPPUserRoleHelper.IsTeacher(this.UserRoleId))
            {
                var teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());
                if (teacher == null || teacher.IdTeacher != course.IdCourse)
                    return View("Error");
            }
            //authorisation check ok, display stuff

            var model = new LectureAttendanceEditViewModel()
            {
                IdCourse = course.IdCourse,
                IdTeacher = course.IdTeacher,
                CourseName = course.Name,
            };
            //get valid students
            var students = DbContext.Groups.Where(g => g.IdCourse == course.IdCourse).ToList();
            var studentNames = new Dictionary<int, string>();
            foreach (var s in students)
                studentNames.Add(s.IdStudent, ZPP_Project.Helpers.StudentHelper.Display(s.IdStudent));
            //get lectures
            var lectures = DbContext.Lectures.Where(l => l.IdCourse == course.IdCourse).ToList();
            foreach(var item in lectures)
            {
                var entry = new LectureAttendanceItemEditViewModel()
                {
                    IdLecture = item.IdLecture,
                    LecuteDate = item.LecuteDate,
                };
                //get attendancy for lectures
                var attendancy = DbContext.Attendance.Where(a => a.IdLecture == item.IdLecture);
                foreach (var a in attendancy)
                {
                    entry.Items.Add(new LectureAttendanceEntryEditViewModel()
                    {
                        Attended = a.Attended,
                        IdStudent = a.IdStudent,
                        IdAttendance = a.IdAttendance,
                        StudentName = studentNames.ContainsKey(a.IdStudent)
                            ? studentNames[a.IdStudent]
                            : ProgramData.VALUE_UNKNOWN,
                    });
                }
                model.Items.Add(entry);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.TEACHER, Roles.COMPANY, Roles.ADMINISTRATOR })]
        public ActionResult AttendanceEdit(int id, LectureAttendanceEditViewModel model)
        {
            //authorization
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null)
                return View("Error");
            if (ZPPUserRoleHelper.IsCompany(this.UserRoleId))
            {
                var company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                if (company == null || company.IdCompany != course.IdCourse)
                    return View("Error");
            }
            else if (ZPPUserRoleHelper.IsTeacher(this.UserRoleId))
            {
                var teacher = DbContext.FindTeacherByUserId(User.Identity.GetUserId<int>());
                if (teacher == null || teacher.IdTeacher != course.IdCourse)
                    return View("Error");
            }
            //authorisation check ok, parse results

            var validStudents = DbContext.Groups.Where(g => g.IdCourse == course.IdCourse).Select(g => g.IdStudent).ToList();
            foreach (var item in model.Items)
            {
                //check if lecture is valid
                if (!DbContext.Lectures.Any(l => l.IdCourse == item.IdLecture && l.IdCourse == course.IdCourse))
                    continue;
                //check attendance of students
                foreach (var entry in item.Items)
                {
                    if (validStudents.Contains(entry.IdStudent))
                    {
                        var attendance = DbContext.Attendance.Where(a => a.IdAttendance == entry.IdAttendance).FirstOrDefault();
                        if (attendance == null || attendance.Attended == entry.Attended)//Double check in case entityframework would not do this
                            continue;
                        attendance.Attended = entry.Attended;
                    }
                }
            }

            DbContext.SaveChanges();
            return View("GenericMessage", new GenericViewModel()
            {
                Title = "Changes saved",
                Message = "attendance for course \"" + course.Name + "\" altered",
                ButtonText = "Back to Courses",
                ButtonHref = @"/Courses"
            });
        }
    }
}
