using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;
using ZPP_Project.Models;
using PagedList;

namespace ZPP_Project.Controllers
{
    [ZPPAuthorize(RolesArray = new string[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
    public class ReportController : ZPPController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [Route("Report/CoursesDetails/{page?}/{pageSize?}")]
        public ActionResult CoursesDetails(int? page, int? pageSize)
        {
            int? idCompany = ViewBag.IdCompany;
            if (idCompany.HasValue)
            {
                IQueryable<V_Course> courses = DbContext.FindCoursesByCompanyId(idCompany.Value);
                IQueryable<V_Teacher> teachers = DbContext.FindTeacherByCompanyId(idCompany.Value);
                List<ReportCourseViewModel> reportCourses = new List<ReportCourseViewModel>(courses.Count());
                foreach (V_Course course in courses)
                {
                    IQueryable<V_Lecture> lectures = DbContext.FindLecturesByCourseId(course.IdCourse);
                    List<ReportLectureViewModel> reportLectures = new List<ReportLectureViewModel>(lectures.Count());
                    foreach (V_Lecture lecture in lectures)
                    {
                        reportLectures.Add(new ReportLectureViewModel()
                        {
                            TeacherFullName = lecture.IdTeacher.HasValue ? TeacherHelper.Display(lecture.IdTeacher.Value) : "Not selected",
                            Date = lecture.LectureDate
                        });
                    }
                    reportCourses.Add(new ReportCourseViewModel()
                    {
                        TeacherFullName = course.IdTeacher.HasValue ? TeacherHelper.Display(course.IdTeacher.Value) : "Not selected",
                        Name = course.Name,
                        Lectures = reportLectures,
                        DateStart = course.DateStart,
                        DateEnd = course.DateEnd,
                        State = ((States.CourseState)course.State).ToString(),
                        StudentsCount = DbContext.FindGroupsByCourseId(course.IdCourse).Count()
                    });
                }
                return View(reportCourses.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
            }
            return RedirectToAction("Index");
        }
    }
}