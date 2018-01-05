using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Models
{
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
            DateStart= baseCourse.DateStart;
            DateEnd = baseCourse.DateEnd;
        }

    }
}