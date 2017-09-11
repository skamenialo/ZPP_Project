using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Models;

namespace ZPP_Project.DataAccess
{
    public class ZppContext : DbContext
    {

#region Properties

        public DbSet<V_Course> Courses { get; set; }
        public DbSet<V_Teacher> Teachers { get; set; }
        public DbSet<V_Company> Companies { get; set; }
        public DbSet<V_Student> Students { get; set; }

#endregion

#region Constructor

        public ZppContext()
#if DATABASE_LOCAL
            : base("ZPPEntities_NoDb")
#elif DATABASE_REMOTE
            : base("ZPPEntities")
#endif
        { }


#endregion

        public static ZppContext Create()
        {
            return new ZppContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }

}