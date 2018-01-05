using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;
using ZPP_Project.Models;
using PagedList;

namespace ZPP_Project.Controllers
{
    [ZPPAuthorize]
    public class StudentController : ZPPController
    {
        // GET: Student
        [ZPPAuthorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<ActionResult> Index(int? page, int? pageSize)
        {
            var list = await DbContext.Students.ToListAsync();
            return View(list.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        // GET: Student/Details/5
        [ZPPAuthorize(Roles = Roles.ADMINISTRATOR + "," + Roles.COMPANY)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            V_Student v_Student = await DbContext.Students.FindAsync(id);
            if (v_Student == null)
                return HttpNotFound();

            if(User.IsInRole(Roles.COMPANY))
            {
                V_Company v_Company = await DbContext.FindCompanyByUserIdAsync(User.Identity.GetUserId<int>());
                bool canDisplay = false;
                foreach (V_Course course in DbContext.FindCoursesByCompanyId(v_Company.IdCompany))
                {
                    if (DbContext.FindGroupsByCourseId(course.IdCourse).Any(g => g.IdStudent == v_Student.IdStudent))
                    {
                        canDisplay = true;
                        break;
                    }
                }
                if (!canDisplay)
                    return HttpNotFound();
            }
            return View(v_Student);
        }

        // GET: Student/Edit/5
        [ZPPAuthorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_Student v_Student = await DbContext.Students.FindAsync(id);
            if (v_Student == null)
            {
                return HttpNotFound();
            }

            return View(EditStudentViewModel.GetFromV_Student(v_Student));
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<ActionResult> Edit(EditStudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                if (await DbContext.Students.FindAsync(student.Id) == null)
                    AddError("Student not exists");
                else
                {
                    DbContext.Entry(new V_StudentInfo()
                    {
                        IdStudent = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Address = student.Address
                    }).State = EntityState.Modified;
                    await DbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(student);
        }

        //// GET: Student/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Student/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(V_Student v_Student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DbContext.Students.Add(v_Student);
        //        await DbContext.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(v_Student);
        //}

        // GET: Student/MyCourses
        [ZPPAuthorize(Roles = Roles.STUDENT)]
        public async Task<ActionResult> MyCourses(int? page, int? pageSize)
        {
            if (User == null || String.IsNullOrWhiteSpace(User.Identity.Name))
                return HttpNotFound();
            V_Student student = await DbContext.FindStudentByUserIdAsync(User.Identity.GetUserId<int>());
            List<V_CourseExtended> list = new List<V_CourseExtended>();
            foreach (var item in DbContext.FindCoursesByStudentId(student.IdStudent))
            {
                list.Add(new V_CourseExtended(item) { IsMember = true,});
            }
            return View(list.ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }
    }
}
