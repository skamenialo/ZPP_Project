using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ZPP_Project.Models
{
    /// <summary>
    /// WARNING: this class should not be referenced, use V_Course instead
    /// </summary>
    [Bind(Exclude = "IdUser")]
    public class Teacher
    {
        public Teacher() { throw new NotSupportedException(); }
        [Key]
        public int IdUser { get; set; }
        [Required]
        [DisplayName("User type")]
        public int UserType { get; set; }
        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Address")]
        public string Address { get; set; }
        [Required]
        [DisplayName("Email address")]
        public string Email { get; set; }
        [Required]
        [DisplayName("Company")]
        public int IdCompany { get; set; }
        [DisplayName("Academic degree")]
        public string Degree { get; set; }
        [DisplayName("Website")]
        public string Website { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
    }

    public class SelectTeacherCompanyViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string Company { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Companies { get; set; }
    }
}