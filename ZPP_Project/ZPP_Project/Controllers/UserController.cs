using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ZPP_Project.Helpers;
using ZPP_Project.Models;
using ZPP_Project.EntityDataModel;
using PagedList;

namespace ZPP_Project.Controllers
{
    [ZPPAuthorize(Roles = Helpers.Roles.ADMINISTRATOR)]
    public class UserController : ZPPController
    {
        private ZppIdentityContext db = new ZppIdentityContext();

        // GET: User
        public ActionResult Index(int? page, int? pageSize)
        {
            return View(db.Users.AsEnumerable().Select(user => DisplayUserViewModel.GetFromZppUser(user)).ToPagedList(page ?? 1, pageSize ?? ProgramData.DEFAULT_PAGE_SIZE)); 
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ZppUser zppUser = await UserManager.FindByIdAsync(id.Value);
            if (zppUser == null)
                return HttpNotFound();

            return View(DisplayUserViewModel.GetFromZppUser(zppUser));
        }

        // GET: User/Create
        public ActionResult Create()
        {
            
            return View(new CreateUserViewModel() { LockoutEnabled = true, UserTypes = GetUserTypes(), AddDetailsManually = false});
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ZppUser { UserName = model.UserName, Email = model.Email, UserType = Int32.Parse(model.UserType), LockoutEnabled = model.LockoutEnabled };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (user.UserType > 1)
                    {
                        if (model.AddDetailsManually)
                        {
                            switch (user.UserType)
                            {
                                case 2:
                                    return RedirectToAction("Create", "Student", new { id = user.UserName });
                                case 3:
                                    return RedirectToAction("Create", "Company", new { id = user.UserName });
                                case 4:
                                case 5:
                                    return RedirectToAction("Create", "Teacher", new { id = user.UserName });
                            }
                        }
                        else
                        {
                            switch (user.UserType)
                            {
                                case 2:
                                    DbContext.Entry(new V_StudentData()
                                    {
                                        IdUser = user.Id,
                                        LastName = " ",
                                        FirstName = " ",
                                        Address = " "
                                    }).State = EntityState.Added;
                                    break;
                                case 3:
                                    DbContext.Entry(new V_CompanyData()
                                    {
                                        IdUser = user.Id,
                                        Name = " ",
                                        Address = " ",
                                        Email = " "
                                    }).State = EntityState.Added;
                                    break;
                                case 4:
                                case 5:
                                    return RedirectToAction("SelectCompany", "Teacher", new { id = user.Id });
                            }
                            DbContext.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            model.UserTypes = GetUserTypes();
            return View(model);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ZppUser zppUser = await UserManager.FindByIdAsync(id.Value);
            if (zppUser == null)
                return HttpNotFound();

            EditUserViewModel model = EditUserViewModel.GetFromZppUser(zppUser);
            model.UserTypes = GetUserTypes();
            return View(model);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ZppUser user = UserManager.FindById(model.UserId);
                if (user != null)
                {
                    ZppUser user2 = null;
                    string[] removeRoles = null;
                    bool userTypeChanged = false;

                    if (!string.IsNullOrEmpty(model.UserName) && user.UserName != model.UserName)
                    {
                        user2 = UserManager.FindByName(model.UserName);
                        if (user2 == null || user2.Equals(user))
                        {
                            user.UserName = model.UserName;
                            user.EmailConfirmed = false;
                        }
                        else
                            AddError("User with given name already exists!");
                    }

                    if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
                    {
                        user2 = UserManager.FindByEmail(model.Email);
                        if (user2 == null || user2.Equals(user))
                        {
                            user.Email = model.Email;
                            user.EmailConfirmed = false;
                        }
                        else
                            AddError("User with given email already exists!");
                    }

                    if (!string.IsNullOrEmpty(model.UserType))
                    {
                        int userType = user.UserType;
                        if (Int32.TryParse(model.UserType, out userType)
                            && userType <= DbContext.UserTypes.Count())
                        {
                            if (user.UserType != 1 && userType > 1)
                            {
                                userTypeChanged = userType != user.UserType;
                                if (userTypeChanged)
                                {
                                    switch (user.UserType)
                                    {
                                        case 2:
                                            if (userType != 5)
                                                removeRoles = new string[] { Helpers.Roles.STUDENT };
                                            break;
                                        case 3:
                                            removeRoles = new string[] { Helpers.Roles.COMPANY };
                                            break;
                                        case 4:
                                            if (userType != 5)
                                                removeRoles = new string[] { Helpers.Roles.TEACHER };
                                            break;
                                        case 5:
                                            if (userType == 2)
                                                removeRoles = new string[] { Helpers.Roles.TEACHER };
                                            else if (userType == 4)
                                                removeRoles = new string[] { Helpers.Roles.STUDENT };
                                            else
                                                removeRoles = new string[] { Helpers.Roles.STUDENT, Helpers.Roles.TEACHER };
                                            break;
                                    }

                                    user.UserType = userType;
                                    user.EmailConfirmed = false;
                                }
                            }
                            else
                                AddError("Could not change" + (user.UserType == 1 ? " " : " to ") + "administrator role!");
                        }
                        else
                            AddError("Wrong user role!");
                    }

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        model.Password = new SHA512PasswordHasher().HashPassword(model.Password);
                        if (!model.Password.Equals(user.PasswordHash))
                            user.PasswordHash = model.Password;
                        else
                        {
                            ModelState.AddModelError("Password", "Password must be different!");
                            model.Password = null;
                            model.ConfirmPassword = null;
                        }
                    }

                    if (!string.IsNullOrEmpty(model.PhoneNumber) && user.PhoneNumber != model.PhoneNumber)
                    {
                        user.PhoneNumber = model.PhoneNumber;
                        user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    }

                    if (model.LockoutEndDateUtc != null && model.LockoutEndDateUtc < DateTime.UtcNow)
                        ModelState.AddModelError("LockoutEndDateUtc", "Invalid date!");
     
                    if (ModelState.IsValid)
                    {
                        if (user.LockoutEndDateUtc != model.LockoutEndDateUtc)
                            user.LockoutEndDateUtc = model.LockoutEndDateUtc;

                        if (user.Banned != model.Banned)
                        {
                            user.Banned = model.Banned;
                            if (model.Banned && user.LockoutEndDateUtc == null)
                                user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(1);
                        }
                        user.AccessFailedCount = 0;
                        user.LockoutEnabled = model.LockoutEnabled;
                        user.TwoFactorEnabled = model.TwoFactorEnabled;
                        
                        UserManager.Update(user);
                        if (removeRoles != null)
                            UserManager.RemoveFromRoles(user.Id, removeRoles);
                        if(userTypeChanged)
                        {
                            switch(user.UserType)
                            {
                                case 2:
                                    V_Student student = DbContext.FindStudentByUserId(user.Id);
                                    if (student == null)
                                        DbContext.Entry(new V_StudentData()
                                        {
                                            IdUser = user.Id,
                                            LastName = " ",
                                            FirstName = " ",
                                            Address = " "
                                        }).State = EntityState.Added;
                                    break;
                                case 3:
                                    V_Company company = DbContext.FindCompanyByUserId(user.Id);
                                    if (company == null)
                                        DbContext.Entry(new V_CompanyData()
                                        {
                                            IdUser = user.Id,
                                            Name = " ",
                                            Address = " ",
                                            Email = " "
                                        }).State = EntityState.Added;
                                    break;
                                case 4:
                                case 5:
                                    V_Teacher teacher = DbContext.FindTeacherByUserId(user.Id);
                                    if (teacher == null)
                                        return RedirectToAction("SelectCompany", "Teacher", new { id = user.Id });
                                    goto case 2;
                            }
                            DbContext.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                    AddError("Could not find user for edit!");
            }
            model.Password = null;
            model.ConfirmPassword = null;
            model.UserTypes = GetUserTypes();
            return View(model);
        }

        // GET: User/Block/5
        public async Task<ActionResult> Block(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ZppUser zppUser = await UserManager.FindByIdAsync(id.Value);
            if (zppUser == null)
                return HttpNotFound();

            if (zppUser.Banned)
            {
                AddError("User already locked");
                return RedirectToAction("Index");
            }
            return View(new BlockUserViewModel() { UserId = zppUser.Id, UserName = zppUser.UserName });
        }

        // POST: User/Block/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Block(int id)
        {
            ZppUser zppUser = await UserManager.FindByIdAsync(id);
            if (zppUser != null)
            {
                zppUser.Banned = true;
                zppUser.LockoutEndDateUtc = DateTime.UtcNow.AddYears(1);
                var result = await UserManager.UpdateAsync(zppUser);
                if (!result.Succeeded)
                    AddErrors(result);
            }
            return RedirectToAction("Index");
        }

        // GET: User/Unlock/5
        public async Task<ActionResult> Unlock(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ZppUser zppUser = await UserManager.FindByIdAsync(id.Value);
            if (zppUser == null)
                return HttpNotFound();

            if (!zppUser.Banned)
            {
                AddError("User already unlocked");
                return RedirectToAction("Index");
            }
            return View(new BlockUserViewModel() { UserId = zppUser.Id, UserName = zppUser.UserName });
        }

        // POST: User/Unlock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Unlock(int id)
        {
            ZppUser zppUser = await UserManager.FindByIdAsync(id);
            if (zppUser != null)
            {
                zppUser.Banned = false;
                zppUser.LockoutEndDateUtc = null;
                var result = await UserManager.UpdateAsync(zppUser);
                if (!result.Succeeded)
                    AddErrors(result);
            }
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetUserTypes()
        {
            List<SelectListItem> userTypes = new List<SelectListItem>();
            foreach (ZPP_Project.EntityDataModel.SL_UserType type in DbContext.UserTypes.ToList())
            {
                userTypes.Add(new SelectListItem
                {
                    Text = ZPPUserRoleHelper.GetUserRoleName(type.IdUserType),
                    Value = type.IdUserType.ToString()
                });
            }
            return userTypes;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}