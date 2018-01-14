using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZPP_Project.Models
{
    public class CreateCompanyViewModel
    {
        //Company related
        [Required]
        [Display(Name = "Company name")]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        [MaxLength(512)]
        public string Address { get; set; }

        [Display(Name = "Contact email")]
        [EmailAddress]
        [MaxLength(256)]
        public string ContactEmail { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserName { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Users { get; set; }
    }
}