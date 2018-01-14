using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Models
{
    public class EditStudentViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        public string Address { get; set; }

        public static EditStudentViewModel GetFromV_Student(V_Student student)
        {
            return new EditStudentViewModel()
            {
                Id = student.IdStudent,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Address = student.Address
            };
        }
    }

}