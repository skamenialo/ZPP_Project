using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZPP_Project.Models
{
    /// <summary>
    /// WARNING: this class should not be referenced, use V_Course instead
    /// </summary>
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
        [System.Web.Mvc.AllowHtml]
        public string Description { get; set; }
    }

    public class CreateTeacherViewModel
    {
        //User related
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Users { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //Teacher related
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        public string Address { get; set; }

        public string Degree { get; set; }

        [Url]
        public string Website { get; set; }

        [System.Web.Mvc.AllowHtml]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string Company { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Companies { get; set; }
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