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
        public DbSet<V_Group> Groups { get; set; }
        public DbSet<SL_UserType> UserTypes { get; set; }
        public DbSet<SL_CourseStates> CouseStates { get; set; }
        public DbSet<SL_CommentState> CommentStates { get; set; }

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

        public V_Company FindCompanyByUserId(int id)
        {
            return Companies.FirstOrDefault(company => company.IdUser == id);
        }

        public async Task<V_Company> FindCompanyByUserIdAsync(int id)
        {
            return await Companies.FirstOrDefaultAsync(company => company.IdUser == id);
        }

        public V_Student FindStudentByUserId(int id)
        {
            return Students.FirstOrDefault(student => student.IdUser == id);
        }

        public async Task<V_Student> FindStudentByUserIdAsync(int id)
        {
            return await Students.FirstOrDefaultAsync(student => student.IdUser == id);
        }

        public V_Teacher FindTeacherByUserId(int id)
        {
            return Teachers.FirstOrDefault(teacher => teacher.IdUser == id);
        }

        public async Task<V_Teacher> FindTeacherByUserIdAsync(int id)
        {
            return await Teachers.FirstOrDefaultAsync(teacher => teacher.IdUser == id);
        }

        public IQueryable<V_Teacher> FindTeacherByCompanyId(int id)
        {
            return Teachers.Where(t => t.IdCompany == id);
        }

        public IQueryable<V_Course> FindCoursesByCompanyId(int id)
        {
            return Courses.Where(c => c.IdCompany == id);
        }

        public IQueryable<V_Course> FindCoursesByStudentId(int id)
        {
            return FindGroupsByStudentId(id).Select(g => Courses.FirstOrDefault(c => c.IdCourse == g.IdCourse));
        }

        public IQueryable<V_Group> FindGroupsByCourseId(int id)
        {
            return Groups.Where(g => g.IdCourse == id);
        }

        public IQueryable<V_Group> FindGroupsByStudentId(int id)
        {
            return Groups.Where(g => g.IdStudent == id);
        }

#endregion

    }

}