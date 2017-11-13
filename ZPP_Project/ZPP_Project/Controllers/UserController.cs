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

namespace ZPP_Project.Controllers
{
    [ZPPAuthorize(Roles = Helpers.Roles.ADMINISTRATOR)]
    public class UserController : ZPPController
    {
        private ZppIdentityContext db = new ZppIdentityContext();

        // GET: User
        public ActionResult Index()
        {
            return View(db.Users.AsEnumerable().Select(user => DisplayUserViewModel.GetFromZppUser(user))); 
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
            
            return View(new CreateUserViewModel() { LockoutEnabled = true, UserTypes = GetUserTypes()});
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
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

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
                    ZppUser user2 = UserManager.FindByName(model.UserName);
                    if (user2 == null || user2.Equals(user))
                    {
                        user2 = UserManager.FindByEmail(model.Email);
                        if (user2 == null || user2.Equals(user))
                        {
                            user2 = new ZppUser()
                            {
                                Id = model.UserId,
                                UserName = model.UserName,
                                AccessFailedCount = 0,
                                Banned = model.Banned,
                                Email = model.Email,
                                LockoutEnabled = model.LockoutEnabled,
                                LockoutEndDateUtc = model.LockoutEndDateUtc,
                                PhoneNumber = model.PhoneNumber,
                                PhoneNumberConfirmed = model.PhoneNumberConfirmed,
                                TwoFactorEnabled = model.TwoFactorEnabled
                            };
                            int userType = user.UserType;
                            if (Int32.TryParse(model.UserType, out userType)
                                && userType <= DbContext.UserTypes.Count())
                            {
                                if (userType != user.UserType)
                                {
                                    user2.UserType = userType;
                                    user2.EmailConfirmed = false;
                                }
                            }
                            else
                                AddError("Wrong user role!");
                            if (!string.IsNullOrEmpty(model.Password))
                            {
                                model.Password = new SHA512PasswordHasher().HashPassword(model.Password);
                                if (!model.Password.Equals(user.PasswordHash))
                                    user2.PasswordHash = model.Password;
                                else
                                {
                                    AddError<EditUserViewModel, string>(model, m => m.Password, "Password must be different!");
                                    model.Password = null;
                                    model.ConfirmPassword = null;
                                }
                            }
                            if (ModelState.IsValid)
                            {
                                UserManager.Update(user2);
                                return RedirectToAction("Index");
                            }
                        }
                        else
                            AddError("User with given email already exists!");
                    }
                    else
                        AddError("User with given name already exists!");
                }
                else
                    AddError("Could not find user for edit!");
            }
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