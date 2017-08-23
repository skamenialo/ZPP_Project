using System;
using System.Collections.Generic;

namespace ZPP_Project.Models
{
    public class Course
    {
        public int IdCourse { get; set; }
        public int IdTeacher { get; set; }
        public int IdCompany { get; set; }
        public string Name { get; set; }
        public int Lectures { get; set; }
        public int State { get; set; }
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}