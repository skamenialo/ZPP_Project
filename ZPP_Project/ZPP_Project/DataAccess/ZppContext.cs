using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Models;
using System.Threading.Tasks;

namespace ZPP_Project.DataAccess
{
    public class ZppContext : DbContext
    {

#region Properties

        public DbSet<V_Course> Courses { get; set; }
        public DbSet<V_Teacher> Teachers { get; set; }
        public DbSet<V_Company> Companies { get; set; }
        public DbSet<V_Student> Students { get; set; }
        public DbSet<SL_UserType> UserTypes { get; set; }

#endregion

#region Constructor

        public ZppContext()
#if DATABASE_LOCAL
            : base("ZPPEntities_NoDb")
#elif DATABASE_REMOTE
            : base("ZPPEntities")
#endif
        { }

        public static ZppContext Create()
        {
            return new ZppContext();
        }

#endregion

#region Public members

        public V_Company FindCompanyById(int id)
        {
            return Companies.FirstOrDefault(company => company.IdUser == id);
        }

        public async Task<V_Company> FindCompanyByIdAsync(int id)
        {
            return await Companies.FirstOrDefaultAsync(company => company.IdUser == id);
        }

        public V_Student FindStudentById(int id)
        {
            return Students.FirstOrDefault(company => company.IdUser == id);
        }

        public async Task<V_Student> FindStudentByIdAsync(int id)
        {
            return await Students.FirstOrDefaultAsync(company => company.IdUser == id);
        }

        public V_Teacher FindTeacherById(int id)
        {
            return Teachers.FirstOrDefault(company => company.IdUser == id);
        }

        public async Task<V_Teacher> FindTeacherByIdAsync(int id)
        {
            return await Teachers.FirstOrDefaultAsync(company => company.IdUser == id);
        }

#endregion

    }

}