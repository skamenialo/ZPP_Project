using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZPP_Project.Models
{
    public class DisplayUserViewModel
    {
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "User role")]
        public int UserType { get; set; }

        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Email confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone number confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        public bool Banned { get; set; }

        [Display(Name = "Lockout enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Lockout end date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Access failed count")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "Two factor enabled")]
        public bool TwoFactorEnabled { get; set; }

        public static DisplayUserViewModel GetFromZppUser(ZppUser user)
        {
            return new DisplayUserViewModel()
            {
                UserId = user.Id,
                UserType = user.UserType,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Banned = user.Banned,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                AccessFailedCount = user.AccessFailedCount,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }
    }
}