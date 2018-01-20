﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Models
{
    /// <summary>
    /// WARNING: this class should not be referenced, use V_Course instead
    /// </summary>
    [Bind(Exclude = "IdCourse")]
    public class Course
    {
        public Course() { throw new NotSupportedException(); }
        [Key]
        public int IdCourse { get; set; }
        [Required]
        [DisplayName("Teacher")]
        public int IdTeacher { get; set; }
        [Required]
        [DisplayName("Company")]
        public int IdCompany { get; set; }
        [Required]
        [DisplayName("Course name")]
        public string Name { get; set; }
        [Required]
        [Range(1, 256, ErrorMessage = "Invalid number of courses")]
        [DisplayName("Number of lectures")]
        public int Lectures { get; set; }
        [Required]
        [DisplayName("State")]
        public int State { get; set; }
        [StringLength(4096, ErrorMessage = "Description is too long")]
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Start date")]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName("End date")]
        public Nullable<System.DateTime> DateEnd { get; set; }
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
}