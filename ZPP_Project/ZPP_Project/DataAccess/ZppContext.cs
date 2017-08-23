using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ZPP_Project.Models;

namespace ZPP_Project.DataAccess
{
    public class ZppContext : DbContext
    {

#region Properties

        public DbSet<Course> Courses { get; set; }

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
        }
    }

}