using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;

namespace ZPP_Project.Models
{
    /// <summary>
    /// WARNING: this class should not be referenced, use V_Course instead
    /// </summary>
    public class CreateCourseViewModel
    {
        [DisplayName("Tutor")]
        public string IdTeacher { get; set; }

        [DisplayName("Company")]
        public string IdCompany { get; set; }

        [Required]
        [DisplayName("Course name")]
        public string Name { get; set; }

        [DisplayName("Lectures")]
        public int Lectures { get; set; }

        [StringLength(4096, ErrorMessage = "Description is too long")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Start date")]
        public Nullable<System.DateTime> DateStart { get; set; }

        [DisplayName("End date")]
        public Nullable<System.DateTime> DateEnd { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Teachers { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Companies { get; set; }
    }

    public class V_CourseExtended : V_Course
    {
        public int? IdStudent { get; set; }
        public bool IsMember { get; set; }

        [Display(Name = "Grade")]
        public double? Grade { get; set; }

        public V_CourseExtended() { }

        public V_CourseExtended(V_Course baseCourse)
        {
            IdCourse = baseCourse.IdCourse;
            IdTeacher = baseCourse.IdTeacher;
            IdCompany = baseCourse.IdCompany;
            Name = baseCourse.Name;
            Lectures = baseCourse.Lectures;
            State = baseCourse.State;
            Description = baseCourse.Description;
            DateStart = baseCourse.DateStart;
            DateEnd = baseCourse.DateEnd;
        }
    }

    public class CourseSignViewModel
    {
        [Required]
        public int IdStudent { get; set; }//@Html.HiddenFor(model => model.HiddenText);

        [Required]
        public int IdCourse { get; set; }

        [Display(Name = "Teacher")]
        public string TeacherFullName { get; set; }

        [Display(Name = "Company")]
        public string CompanyFullName { get; set; }

        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Lectures")]
        public int Lectures { get; set; }

        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateStart { get; set; }

        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateEnd { get; set; }
    }

    public class AttendanceViewModel
    {
        public List<LectureAttendanceViewModel> Lectures { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }

    }

    public class LectureAttendanceViewModel
    {
        public int IdAttendance { get; set; }
        public int IdLecture { get; set; }
        public bool Attended { get; set; }
        public Nullable<System.DateTime> LecuteDate { get; set; }
    }

    public class GradeEditViewModel
    {
        public string CourseName { get; set; }
        public List<GradeEditItemViewModel> Items { get; set; }
        public GradeEditViewModel()
        {
            this.Items = new List<GradeEditItemViewModel>();
        }
    }

    public class GradeEditItemViewModel
    {
        public int IdGrade { get; set; }

        public int IdStudent { get; set; }

        [Display(Name = "Student")]
        public string StudentName { get; set; }
        
        public int IdCourse { get; set; }

        [Display(Name = "Grade")]
        [Range(3, 5)]
        public decimal? Grade { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }

        public int IdTeacher { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }

    /// <summary>
    /// Represents an attendance for entire course
    /// </summary>
    public class LectureAttendanceEditViewModel
    {
        public string CourseName { get; set; }
        public int IdCourse { get; set; }
        public int IdTeacher { get; set; }
        public List<LectureAttendanceItemEditViewModel> Items { get; set; }
        public LectureAttendanceEditViewModel()
        {
            this.Items = new List<LectureAttendanceItemEditViewModel>();
        }
    }

    /// <summary>
    /// Represents an attendance for a single lecture within a course
    /// </summary>
    public class LectureAttendanceItemEditViewModel
    {
        public int IdLecture { get; set; }
        public Nullable<System.DateTime> LecuteDate { get; set; }
        public List<LectureAttendanceEntryEditViewModel> Items { get; set; }
        public LectureAttendanceItemEditViewModel()
        {
            this.Items = new List<LectureAttendanceEntryEditViewModel>();
       } 
    }

    //Represents an attendance of a single student for a single lecture within a course
    public class LectureAttendanceEntryEditViewModel
    {
        public bool Attended { get; set; }
        public string StudentName { get; set; }
        public int IdStudent { get; set; }
        public int IdAttendance { get; set; }

    }
}