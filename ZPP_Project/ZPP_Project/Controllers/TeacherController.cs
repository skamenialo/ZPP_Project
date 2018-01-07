using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using PagedList;
using ZPP_Project.Models;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ZPP_Project.Controllers
{
    public class TeacherController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public TeacherController()
            : base() { }

        public TeacherController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        // GET: Teacher
        public ActionResult Index(int? page, int? pageSize)
        {
            return View("Index", DbContext.Teachers.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Teachers.Where(teacher => teacher.IdTeacher == id).FirstOrDefault());
        }

        [ZPPAuthorize(Roles=Roles.ADMINISTRATOR)]
        public ActionResult SelectCompany(int id)
        {
            return View(new SelectTeacherCompanyViewModel() { Id = id, Company = "-1", Companies = GetCompanies() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<ActionResult> SelectCompany(SelectTeacherCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                int companyId = 0;
                if (ModelState.IsValid && int.TryParse(model.Company, out companyId))
                {
                    V_Company company = DbContext.Companies.Find(companyId);
                    if (company != null)
                    {
                        ZppUser user = await UserManager.FindByIdAsync(model.Id);
                        if (user != null && ZPPUserRoleHelper.IsTeacher(user.UserType))
                        {
                            if (ZPPUserRoleHelper.IsStudent(user.UserType) && DbContext.FindStudentByUserId(model.Id) == null)
                            {
                                DbContext.Entry(new V_StudentData()
                                {
                                    IdUser = model.Id,
                                    FirstName = " ",
                                    LastName = " ",
                                    Address = " "
                                }).State = EntityState.Added;
                            }

                            V_Teacher teacher = DbContext.FindTeacherByUserId(model.Id);
                            if (teacher == null)
                            {
                                DbContext.Entry(new V_TeacherData()
                                {
                                    IdUser = model.Id,
                                    IdCompany = company.IdCompany,
                                    FirstName = " ",
                                    LastName = " ",
                                    Address = " "
                                }).State = EntityState.Added;
                            }
                            else if (teacher.IdCompany != company.IdCompany)
                            {
                                DbContext.Entry(new V_TeacherData()
                                {
                                    IdTeacher = teacher.IdTeacher,
                                    IdUser = teacher.IdUser,
                                    IdCompany = company.IdCompany,
                                    FirstName = teacher.FirstName,
                                    LastName = teacher.LastName,
                                    Address = teacher.Address
                                }).State = EntityState.Modified;
                            }
                            DbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                            AddError("Wrong user");
                    }
                    else
                        AddError("Selected company not exists");
                }
            }
            model.Companies = GetCompanies();
            return View(model);
        }

        private List<SelectListItem> GetCompanies()
        {
            List<SelectListItem> companies = new List<SelectListItem>();
            foreach (V_Company company in DbContext.Companies)
            {
                companies.Add(new SelectListItem()
                {
                    Text = company.Name,
                    Value = company.IdCompany.ToString()
                });
            }
            return companies;
        }
    }
}