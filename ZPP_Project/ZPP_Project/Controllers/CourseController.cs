﻿using Microsoft.AspNet.Identity;
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
using System.Data.Entity;
using ZPP_Project.States;

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
            var lectures = new List<V_Lecture>(DbContext.FindLecturesByCourseId(course.IdCourse));
            var lectureIds = lectures.Select(l => l.IdLecture);
            var attendanceModel = new AttendanceViewModel()
            {
                CourseName = course.Name,
                StudentName = ZPP_Project.Helpers.StudentHelper.Display(student),
                Lectures = DbContext.Attendance.Where(a => a.IdStudent == student.IdStudent && lectureIds.Contains(a.IdLecture) == true).ToList().Select(a => new LectureAttendanceViewModel()
                {
                    Attended = a.Attended,
                    IdAttendance = a.IdAttendance,
                    LectureDate = lectures.FirstOrDefault(l => l.IdLecture == a.IdLecture).LectureDate
                }).ToList()
            };
            attendanceModel.Lectures.Sort((l1, l2) => DateTime.Compare(l1.LectureDate ?? DateTime.MinValue, l2.LectureDate ?? DateTime.MinValue));
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
                IdTeacher = course.IdTeacher.Value,
                CourseName = course.Name,
            };
            //get valid students
            var students = DbContext.Groups.Where(g => g.IdCourse == course.IdCourse).ToList();
            var studentNames = new Dictionary<int, string>();
            foreach (var s in students)
                studentNames.Add(s.IdStudent, ZPP_Project.Helpers.StudentHelper.Display(s.IdStudent));
            //get lectures
            var lectures = DbContext.FindLecturesByCourseId(course.IdCourse).ToList();
            foreach(var item in lectures)
            {
                var entry = new LectureAttendanceItemEditViewModel()
                {
                    IdLecture = item.IdLecture,
                    LectureDate = item.LectureDate,
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
            model.Items.Sort((l1, l2) => DateTime.Compare(l1.LectureDate??DateTime.MinValue, l2.LectureDate??DateTime.MinValue));
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

        #region Create

        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Create()
        {
            TempData.Remove(Keys.CREATE_COURSE_MODEL);
            ViewBag.CourseAction = "Create";

            CreateEditCourseViewModel model = new CreateEditCourseViewModel() { Lectures = new LectureCreateEditItemViewModel[0] };
            AddDataToCreateCourseViewModel(model);
            return View("CreateEdit", model);
        }

        [HttpPost, ZPPSubmitName("create")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Create(CreateEditCourseViewModel model)
        {
            ViewBag.CourseAction = "Create";

            if (ModelState.IsValid)
            {
                if (DbContext.Courses.FirstOrDefault((c) => c.Name.Equals(model.Name)) != null)
                    AddError("Course with given name already exists.");

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
                    AddError("Could not find company");
                else
                {
                    id = 0;
                    V_Teacher teacher = null;
                    if (!string.IsNullOrWhiteSpace(model.IdMainTeacher) && !model.IdMainTeacher.Equals("none"))
                    {
                        if (int.TryParse(model.IdMainTeacher, out id))
                        {
                            teacher = DbContext.Teachers.FirstOrDefault(t => t.IdCompany == company.IdCompany && t.IdTeacher == id);
                            if (teacher == null)
                                AddError("Teacher not exist");
                        }
                        else
                            AddError("Parsing error");
                    }

                    if (ModelState.IsValid)
                    {
                        if (model.Lectures == null || model.Lectures.Count() == 0)
                        {
                            TempData[Keys.CREATE_COURSE_MODEL] = model;
                            return View("CreateEditNoLectures");
                        }
                        else
                            return UpdateCourse(model, company, teacher, null);
                    }
                }
            }

            AddDataToCreateCourseViewModel(model);
            return View("CreateEdit", model);
        }
        
        [HttpPost, ZPPSubmitName("back_to_create")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult CreateBack()
        {
            CreateEditCourseViewModel model = TempData[Keys.CREATE_COURSE_MODEL] as CreateEditCourseViewModel;
            if (model == null)
                return RedirectToAction("Create");

            ViewBag.CourseAction = "Create";
            AddDataToCreateCourseViewModel(model);
            return View("CreateEdit", model);
        }

        #endregion

        #region Edit

        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Edit(int? id)
        {
            TempData.Remove(Keys.EDIT_COURSE_MODEL);
            if (!id.HasValue)
                RedirectToAction("Index");

            V_Course course = DbContext.Courses.Find(id);
            if(course == null)
                RedirectToAction("Index");

            CreateEditCourseViewModel model = CreateEditCourseViewModel.GetFromV_Course(course);
            List<LectureCreateEditItemViewModel> lectures = new List<LectureCreateEditItemViewModel>();
            int i = 0;
            foreach (V_Lecture lecture in DbContext.FindLecturesByCourseId(course.IdCourse))
                lectures.Add(new LectureCreateEditItemViewModel()
                    {
                        Index = i++,
                        IdTeacher = lecture.IdTeacher.HasValue ? lecture.IdTeacher.ToString() : "none",
                        Date = lecture.LectureDate
                    });
            model.Lectures = lectures.ToArray();
            AddDataToCreateCourseViewModel(model);

            ViewBag.CourseAction = "Edit";
            return View("CreateEdit", model);
        }

        [HttpPost, ZPPSubmitName("edit")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Edit(CreateEditCourseViewModel model)
        {
            ViewBag.CourseAction = "Edit";

            if (!model.IdCourse.HasValue)
                AddError("Could not find course");

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
                    AddError("Could not find company");
                else
                {
                    V_Course course = DbContext.Courses.Find(model.IdCourse.Value);
                    if (course == null || course.IdCompany != company.IdCompany)
                        AddError("Could not find course");
                    else
                    {
                        if (DbContext.Courses.FirstOrDefault((c) => c.IdCourse != course.IdCourse && c.Name.Equals(model.Name)) != null)
                            AddError("Course with given name already exists.");

                        if (course.State > (int)States.CourseState.Opened)
                            AddError("Could not edit started/closed course");
                    }

                    id = 0;
                    V_Teacher teacher = null;
                    if (!string.IsNullOrWhiteSpace(model.IdMainTeacher) && !model.IdMainTeacher.Equals("none"))
                    {
                        if (int.TryParse(model.IdMainTeacher, out id))
                        {
                            teacher = DbContext.Teachers.FirstOrDefault(t => t.IdCompany == company.IdCompany && t.IdTeacher == id);
                            if (teacher == null)
                                AddError("Teacher not exist");
                        }
                        else
                            AddError("Parsing error");
                    }

                    if (ModelState.IsValid)
                    {
                        if (model.Lectures == null || model.Lectures.Count() == 0)
                        {
                            TempData[Keys.EDIT_COURSE_MODEL] = model;
                            return View("CreateEditNoLectures");
                        }
                        else
                            return UpdateCourse(model, company, teacher, course);
                    }
                }
            }

            AddDataToCreateCourseViewModel(model);
            return View("CreateEdit", model);
        }

        [HttpPost, ZPPSubmitName("back_to_edit")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult EditBack()
        {
            CreateEditCourseViewModel model = TempData[Keys.EDIT_COURSE_MODEL] as CreateEditCourseViewModel;
            if (model == null)
                return RedirectToAction("Edit");

            ViewBag.CourseAction = "Edit";
            AddDataToCreateCourseViewModel(model);
            return View("CreateEdit", model);
        }
        
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult ChangeState(int id)
        {
            var course = DbContext.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (course == null)
                return View("Error");
            if (ZPPUserRoleHelper.IsCompany(this.UserRoleId))
            {
                var company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                if (company == null || company.IdCompany != course.IdCompany)
                    return View("Error");
            }
            var model = new ChangeCourseStateViewModel()
            {
                IdCourse = course.IdCourse,
                CurrentState = course.State,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult ChangeState(ChangeCourseStateViewModel model)
        {
            var course = DbContext.Courses.Where(c => c.IdCourse == model.IdCourse).FirstOrDefault();
            if (course == null)
                return View("Error");
            if (ZPPUserRoleHelper.IsCompany(this.UserRoleId))
            {
                var company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
                if (company == null || company.IdCompany != course.IdCompany)
                    return View("Error");
            }
            string error = null;
            if (model.CurrentState == 0)
            {
                error = "Pick a value!";
            }
            else if (course.State != model.CurrentState)
            {
                if (model.CurrentState == 1)
                {
                    var studentCount = DbContext.Groups.Where(g => g.IdCourse == course.IdCourse).Count();
                    if (studentCount > 0)
                        error = "You can't hide this course when there are students signed in to it!";
                }
            }
            bool wasError = !String.IsNullOrEmpty(error);
            if (!wasError)
            {
                course.State = model.CurrentState;
                DbContext.SaveChanges();
            }
            return View("GenericMessage", new GenericViewModel()
            {
                Title = wasError
                ? "State not changed"
                : "State changed",
                Message = wasError
                ? error
                : "State for this course was changed to " + (CourseState)course.State,
                ButtonText = "Back to Courses",
                ButtonHref = @"/Courses"
            });
        }

        #endregion

        #region Create/Edit

        [HttpPost, ZPPSubmitName("add_lecture")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult AddLecture(CreateEditCourseViewModel model)
        {
            return CreateEditLecture(model, new LectureCreateEditItemViewModel() { Teachers = GetCompanyTeachers(ViewBag.IdCompany, true), Edit = true });
        }

        [HttpPost, ZPPSubmitName("edit_lecture")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult EditLecture(CreateEditCourseViewModel model, int? id)
        {
            if (id == null || model.Lectures == null || model.Lectures.Length <= id.Value)
            {
                ViewBag.CourseAction = GetActionName(false);
                AddDataToCreateCourseViewModel(model);
                return View("CreateEdit", model);
            }

            return CreateEditLecture(model, new LectureCreateEditItemViewModel()
            {
                Index = id.Value,
                Date = model.Lectures[id.Value].Date,
                IdTeacher = model.Lectures[id.Value].IdTeacher,
                Teachers = GetCompanyTeachers(ViewBag.IdCompany, true),
                Edit = true
            });
        }

        private ActionResult CreateEditLecture(CreateEditCourseViewModel model, LectureCreateEditItemViewModel lectureModel)
        {
            CreateEditCourseViewModel createEditModel = null;
            string actionName = GetActionName(true);
            if (actionName.Equals("create"))
            {
                createEditModel = TempData.Peek(Keys.CREATE_COURSE_MODEL) as CreateEditCourseViewModel;
                if (createEditModel == null && model.IdCourse == null)
                    TempData[Keys.CREATE_COURSE_MODEL] = model;
            }
            else if (actionName.Equals("edit"))
            {
                createEditModel = TempData.Peek(Keys.EDIT_COURSE_MODEL) as CreateEditCourseViewModel;
                if (createEditModel == null && model.IdCourse != null)
                    TempData[Keys.EDIT_COURSE_MODEL] = model;
            }

            ViewBag.CourseAction = GetActionName(false);
            return View("CreateEditLecture", lectureModel);
        }

        [HttpPost, ZPPSubmitName("confirm_lecture")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult ConfirmLecture(LectureCreateEditItemViewModel model, int? id)
        {
            ModelState.Remove("id");
            CreateEditCourseViewModel createEditModel = null;
            string actionName = GetActionName(true);
            if (actionName.Equals("create"))
                createEditModel = TempData[Keys.CREATE_COURSE_MODEL] as CreateEditCourseViewModel;
            else if (actionName.Equals("edit"))
                createEditModel = TempData[Keys.EDIT_COURSE_MODEL] as CreateEditCourseViewModel;
            if (createEditModel == null)
                return RedirectToAction(actionName);

            ViewBag.CourseAction = GetActionName(false);

            //check date
            //check teacher
            if (ModelState.IsValid)
            {
                List<LectureCreateEditItemViewModel> lectures;
                if (createEditModel.Lectures == null)
                    lectures = new List<LectureCreateEditItemViewModel>();
                else
                    lectures = createEditModel.Lectures.ToList();
                if (id.HasValue && id < lectures.Count)
                {
                    lectures[id.Value] = model;
                }
                else
                {
                    model.Index = lectures.Count;
                    lectures.Add(model);
                }
                createEditModel.Lectures = lectures.ToArray();

                AddDataToCreateCourseViewModel(createEditModel);
                return View("CreateEdit", createEditModel);
            }
            model.Teachers = GetCompanyTeachers(ViewBag.IdCompany);
            model.Edit = true;
            return View("CreateEditLecture", model);
        }

        [HttpPost, ZPPSubmitName("remove_lecture")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult RemoveLecture(CreateEditCourseViewModel model, int? id)
        {
            ViewBag.CourseAction = GetActionName(false);

            AddDataToCreateCourseViewModel(model);
            if (id == null || model.Lectures == null || model.Lectures.Length < id.Value)
                return View("CreateEdit", model);

            List<LectureCreateEditItemViewModel> lectures = model.Lectures.ToList();
            lectures.RemoveAt(id.Value);
            for (int i = 0; i < lectures.Count; i++)
                lectures[i].Index = i;
            model.Lectures = lectures.ToArray();
            return View("CreateEdit", model);
        }

        [HttpPost, ZPPSubmitName("no_lectures")]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult CreateEditWithoutLectures()
        {
            CreateEditCourseViewModel createEditModel = null;
            string actionName = GetActionName(true);
            if (actionName.Equals("create"))
                createEditModel = TempData[Keys.CREATE_COURSE_MODEL] as CreateEditCourseViewModel;
            else if (actionName.Equals("edit"))
                createEditModel = TempData[Keys.EDIT_COURSE_MODEL] as CreateEditCourseViewModel;
            if (createEditModel == null)
                return RedirectToAction(actionName);

            ViewBag.CourseAction = GetActionName(false);
            int id = 0;
            V_Company company = null;
            if (ZPPUserRoleHelper.IsCompany(UserRoleId))
                company = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>());
            else
            {
                if (int.TryParse(createEditModel.IdCompany, out id))
                    company = DbContext.Companies.Find(id);
                else
                    AddError("Parsing error");
            }

            if (company == null)
                AddError("Could not find company");
            else
            {
                id = 0;
                V_Teacher teacher = null;
                if (!string.IsNullOrWhiteSpace(createEditModel.IdMainTeacher) && !createEditModel.IdMainTeacher.Equals("none"))
                {
                    if (int.TryParse(createEditModel.IdMainTeacher, out id))
                    {
                        teacher = DbContext.Teachers.FirstOrDefault(t => t.IdCompany == company.IdCompany && t.IdTeacher == id);
                        if (teacher == null)
                            AddError("Teacher not exist");
                    }
                    else
                        AddError("Parsing error");
                }

                id = 0;
                V_Course course = null;
                if (createEditModel.IdCourse.HasValue)
                {
                    course = DbContext.Courses.Find(createEditModel.IdCourse.Value);
                    if (course == null || course.IdCompany != company.IdCompany)
                        AddError("Could not find course");
                    else
                    {
                        if (DbContext.Courses.FirstOrDefault((c) => c.IdCourse != course.IdCourse && c.Name.Equals(createEditModel.Name)) != null)
                            AddError("Course with given name already exists.");

                        if (course.State > (int)States.CourseState.Opened)
                            AddError("Could not edit started/closed course");
                    }
                }

                if (ModelState.IsValid)
                    return UpdateCourse(createEditModel, company, teacher, course);
            }
            AddDataToCreateCourseViewModel(createEditModel);
            return View("CreateEdit", createEditModel);
        }

        /// <summary>
        /// The last step in creating course - commiting
        /// This method trust that everything were validated before
        /// </summary>
        private ActionResult UpdateCourse(CreateEditCourseViewModel model, V_Company company, V_Teacher teacher, V_Course course)
        {
            if (course == null)
            {
                course = new V_Course()
                {
                    IdCompany = company.IdCompany,
                    DateEnd = model.DateEnd,
                    DateStart = model.DateStart,
                    Description = model.Description,
                    Lectures = model.Lectures == null ? 0 : model.Lectures.Length,
                    Name = model.Name,
                    State = (int)States.CourseState.Created,
                };
                if (teacher != null)
                    course.IdTeacher = teacher.IdTeacher;
                DbContext.Entry(course).State = EntityState.Added;
            }
            else
            {
                course.IdCompany = company.IdCompany;
                course.DateEnd = model.DateEnd;
                course.DateStart = model.DateStart;
                course.Description = model.Description;
                course.Lectures = model.Lectures == null ? 0 : model.Lectures.Length;
                course.Name = model.Name;
                course.IdTeacher = teacher != null ? (int?)teacher.IdTeacher : null;
            }
            DbContext.SaveChanges();

            if (model.Lectures != null)
            {
                List<V_Lecture> lectures = DbContext.FindLecturesByCourseId(course.IdCourse).ToList();
                if (lectures.Count > 0)
                {
                    bool addLectures = lectures.Count < model.Lectures.Length; //otherwise remove
                    int lecturesCount = addLectures ? model.Lectures.Length : lectures.Count;
                    for (int i = 0; i < lecturesCount; i++)
                    {
                        if ((addLectures && i < lectures.Count) || (!addLectures && i < model.Lectures.Length)) //modify
                            UpdateLectureInDB(lectures[i], model.Lectures[i], course.IdCourse);
                        else if (addLectures) //add
                            AddLectureToDB(model.Lectures[i], course.IdCourse);
                        else //remove
                            DbContext.Entry(lectures[i]).State = EntityState.Deleted;
                    }
                }
                else
                {
                    foreach (LectureCreateEditItemViewModel item in model.Lectures)
                        AddLectureToDB(item, course.IdCourse);
                }
                DbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private void AddLectureToDB(LectureCreateEditItemViewModel item, int idCourse)
        {
            V_Lecture lecture = new V_Lecture()
            {
                IdCourse = idCourse,
                LectureDate = item.Date
            };
            int id = 0;
            if (!(string.IsNullOrWhiteSpace(item.IdTeacher) || item.IdTeacher == "none") && int.TryParse(item.IdTeacher, out id))
                lecture.IdTeacher = id;
            DbContext.Entry(lecture).State = EntityState.Added;
        }

        private void UpdateLectureInDB(V_Lecture lecture, LectureCreateEditItemViewModel item, int idCourse)
        {
            lecture.IdCourse = idCourse;
            lecture.LectureDate = item.Date;
            int id = 0;
            if (!(string.IsNullOrWhiteSpace(item.IdTeacher) || item.IdTeacher == "none") && int.TryParse(item.IdTeacher, out id))
                lecture.IdTeacher = id;
            else
                lecture.IdTeacher = null;
            DbContext.Entry(lecture).State = EntityState.Modified;
        }

        private void AddDataToCreateCourseViewModel(CreateEditCourseViewModel model)
        {
            if (ZPPUserRoleHelper.IsAdministrator(UserRoleId))
                model.Companies = GetCompanies();
            if (ZPPUserRoleHelper.IsCompany(UserRoleId))
                model.Teachers = GetCompanyTeachers(ViewBag.IdCompany, true);
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