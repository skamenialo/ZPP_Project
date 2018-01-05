using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZPP_Project.Models
{
    public class CourseSignViewModel
    {
        [Required]
        public int StudentId { get; set; }//@Html.HiddenFor(model => model.HiddenText);

        [Required]
        public int IdCourse { get; set; }

        [Display(Name = "Teacher")]
        public string TeacherFullName { get; set; }

        public int IdCompany { get; set; }

        [Display(Name = "Company")]
        public string CompanyFullName { get; set; }

        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Lectures")]
        public int Lectures { get; set; }

        [Display(Name = "Start date")]
        public Nullable<System.DateTime> DateStart { get; set; }

        [Display(Name = "End date")]
        public Nullable<System.DateTime> DateEnd { get; set; }
    }
}