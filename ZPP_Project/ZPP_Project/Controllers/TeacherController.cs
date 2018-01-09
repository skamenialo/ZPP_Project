using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;
using ZPP_Project.Models;

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

        [ZPPAuthorize(RolesArray = new string[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public ActionResult Create(string id)
        {
            if(ZPPUserRoleHelper.IsAdministrator(UserRoleId))
                return View(new CreateTeacherViewModel() { UserName = id ?? "", Users =  GetUnusedTeachers(), Companies = GetCompanies() });
            else
                return View(new CreateTeacherViewModel() { UserName = id ?? "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(RolesArray = new string[] { Roles.ADMINISTRATOR, Roles.COMPANY })]
        public async Task<ActionResult> Create(CreateTeacherViewModel model)
        {
            if (ZPPUserRoleHelper.IsAdministrator(UserRoleId))
            {
                ModelState.Remove("Email");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
            else
                ModelState.Remove("Company");

            if (ModelState.IsValid)
            {
                int companyId = 0;
                ZppUser user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    if (ZPPUserRoleHelper.IsCompany(UserRoleId))
                    {
                        //check email
                        user = new ZppUser() { UserName = model.UserName, Email = model.Email, UserType = 4 };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (!result.Succeeded)
                            AddErrors(result);
                        else
                            companyId = DbContext.FindCompanyByUserId(User.Identity.GetUserId<int>()).IdCompany;
                    }
                    else
                        AddError("User with given name not exists.");
                }
                else if (ZPPUserRoleHelper.IsAdministrator(UserRoleId))
                {
                    if (!ZPPUserRoleHelper.IsTeacher(user.UserType))
                        AddError("User is not in Teacher role");

                    if (DbContext.FindTeacherByUserId(user.Id) != null)
                        AddError("User already attached to the teacher");

                    if (!int.TryParse(model.Company, out companyId) || DbContext.Companies.Find(companyId) == null)
                        AddError("Company not exists");
                }
                else
                    AddError("User already exists");

                if (ModelState.IsValid)
                {
                    DbContext.Entry(new V_TeacherData()
                    {
                        IdUser = user.Id,
                        IdCompany = companyId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                    }).State = EntityState.Added;
                    DbContext.SaveChanges();
                    return await GenerateEmailConfirmation(user.Id);
                }
            }

            if (ZPPUserRoleHelper.IsAdministrator(UserRoleId))
            {
                model.Users = GetUnusedTeachers();
                model.Companies = GetCompanies();
            }
            return View(model);
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

        private List<SelectListItem> GetUnusedTeachers()
        {
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (ZppUser user in UserManager.GetTeachers().Where(t => DbContext.FindTeacherByUserId(t.Id) == null))
            {
                users.Add(new SelectListItem
                {
                    Text = user.UserName,
                    Value = user.UserName
                });
            }
            return users;
        }
    }
}