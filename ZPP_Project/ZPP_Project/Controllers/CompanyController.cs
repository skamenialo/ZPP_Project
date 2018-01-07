using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Helpers;
using ZPP_Project.Models;
using PagedList;

namespace ZPP_Project.Controllers
{
    public class CompanyController : ZPP_Project.Helpers.ZPPController
    {
        #region Constructor

        public CompanyController()
            : base() { }

        public CompanyController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager) { }

        #endregion

        // GET: Company
        public ActionResult Index(int? page, int? pageSize)
        {
            return View("Index", DbContext.Companies.ToList().ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE));
        }

        public ActionResult Details(int id)
        {
            return View("Details", DbContext.Companies.Where(company => company.IdCompany == id).FirstOrDefault());
        }

        [ZPPAuthorize(Roles = Helpers.Roles.ADMINISTRATOR)]
        public ActionResult Create()
        {
            if (!this.Request.IsAuthenticated)
            {
                return View("Error");
            }

            return View(new CreateCompanyViewModel() { UserName = "", Users = GetUnusedCompanies() });
        }

        //POST: Company/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ZPPAuthorize(Roles = Helpers.Roles.ADMINISTRATOR)]
        public async Task<ActionResult> Create(CreateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                ZppUser user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    AddError("User with given name not exists.");
                }
                else
                {
                    if (!ZPPUserRoleHelper.IsCompany(user.UserType))
                        AddError("User is not in Company role");

                    if (DbContext.FindCompanyByUserId(user.Id) != null)
                        AddError("User already attached to the company");

                    if (await DbContext.Companies.FirstOrDefaultAsync((c) => c.Name.Equals(model.Name)) != null)
                        AddError("Company with given name already exists.");

                    if(ModelState.IsValid)
                    {
                        DbContext.Entry(new V_CompanyData()
                        {
                            IdUser = user.Id,
                            Name = model.Name,
                            Address = model.Address,
                            Email = string.IsNullOrWhiteSpace(model.ContactEmail) ? user.Email : model.ContactEmail
                        }).State = EntityState.Added;
                        DbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            model.Users = GetUnusedCompanies();
            return View(model);
        }

        private List<SelectListItem> GetUnusedCompanies()
        {
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (ZppUser user in UserManager.GetCompanies().Where(c => DbContext.FindCompanyByUserId(c.Id) == null))
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