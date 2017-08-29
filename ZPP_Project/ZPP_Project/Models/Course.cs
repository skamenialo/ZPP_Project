using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
}