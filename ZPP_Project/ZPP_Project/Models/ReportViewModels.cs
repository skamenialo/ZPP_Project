using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZPP_Project.Models
{
    public class ReportCourseViewModel
    {
        [Display(Name = "Teacher")]
        public string TeacherFullName { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public IEnumerable<ReportLectureViewModel> Lectures { get; set; }

        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateStart { get; set; }

        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateEnd { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [DisplayName("Signed")]
        public int StudentsCount { get; set; }
    }

    public class ReportLectureViewModel
    {
        [Display(Name = "Date")]
        public Nullable<System.DateTime> Date { get; set; }

        [DisplayName("Teacher")]
        public string TeacherFullName { get; set; }
    }
}