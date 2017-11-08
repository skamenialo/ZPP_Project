using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ZPP_Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ZppUser : IdentityUser<int, ZPPUserLogin, ZPPUserRole, ZPPUserClaim>
    {
        [Required]
        public int UserType { get; set; }

        [Required]
        public bool Banned { get; set; } 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ZPPRole : IdentityRole<int, ZPPUserRole> { }

    public class ZPPUserRole : IdentityUserRole<int> { }

    public class ZPPUserClaim : IdentityUserClaim<int> { }

    public class ZPPUserLogin : IdentityUserLogin<int> { }

    public class ZppIdentityContext : IdentityDbContext<ZppUser, ZPPRole, int, ZPPUserLogin, ZPPUserRole, ZPPUserClaim>
    {
        //Identity and Authorization
        public DbSet<ZPPUserLogin> UserLogins { get; set; }
        public DbSet<ZPPUserClaim> UserClaims { get; set; }
        public DbSet<ZPPUserRole> UserRoles { get; set; }

        #region Constructor

        public ZppIdentityContext()
#if DATABASE_LOCAL
            : base("IdentityConnection_NoDb")
#elif DATABASE_REMOTE
            : base("IdentityConnection")
#endif
        { }

        #endregion

        public static ZppIdentityContext Create()
        {
            return new ZppIdentityContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ZppUser>().ToTable("Users");
            modelBuilder.Entity<ZPPRole>().ToTable("SL_UserType");
            modelBuilder.Entity<ZPPUserRole>().ToTable("EF_UserRoles");
            modelBuilder.Entity<ZPPUserClaim>().ToTable("EF_UserClaims");
            modelBuilder.Entity<ZPPUserLogin>().ToTable("EF_UserLogins");

            modelBuilder.Entity<ZppUser>().Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ZPPRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ZPPUserClaim>().Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ZppUser>().Property(p => p.Id).HasColumnName("IdUser");
            modelBuilder.Entity<ZppUser>().Property(p => p.EmailConfirmed).HasColumnName("Active");
            modelBuilder.Entity<ZppUser>().Property(p => p.AccessFailedCount).HasColumnName("EF_AccessFailedCount");
            modelBuilder.Entity<ZppUser>().Property(p => p.LockoutEnabled).HasColumnName("EF_LockoutEnabled");
            modelBuilder.Entity<ZppUser>().Property(p => p.LockoutEndDateUtc).HasColumnName("EF_LockoutEndDateUtc");
            modelBuilder.Entity<ZppUser>().Property(p => p.PhoneNumber).HasColumnName("EF_PhoneNumber");
            modelBuilder.Entity<ZppUser>().Property(p => p.PhoneNumberConfirmed).HasColumnName("EF_PhoneNumberConfirmed");
            modelBuilder.Entity<ZppUser>().Property(p => p.SecurityStamp).HasColumnName("EF_SecurityStamp");
            modelBuilder.Entity<ZppUser>().Property(p => p.TwoFactorEnabled).HasColumnName("EF_TwoFactorEnabled");
            modelBuilder.Entity<ZppUser>().Property(p => p.UserName).HasColumnName("Login");

            modelBuilder.Entity<ZPPRole>().Property(p => p.Id).HasColumnName("IdUserType");
        }
    }
}