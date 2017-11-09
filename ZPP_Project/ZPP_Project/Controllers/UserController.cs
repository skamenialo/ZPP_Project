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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZppUser zppUser = await db.Users.FirstAsync(u => u.Id == id);
            if (zppUser == null)
            {
                return HttpNotFound();
            }
            return View(DisplayUserViewModel.GetFromZppUser(zppUser));
        }

        // GET: User/Create
        public ActionResult Create()
        {
            List<SelectListItem> userTypes = new List<SelectListItem>();
            foreach (ZPP_Project.EntityDataModel.SL_UserType type in DbContext.UserTypes.ToList())
            {
                userTypes.Add(new SelectListItem
                {
                    Text = type.Name,
                    Value = type.IdUserType.ToString()
                });
            }
            return View(new CreateUserViewModel() { LockoutEnabled = true, UserTypes = userTypes});
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
                    return View("Index");
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZppUser zppUser = await db.Users.FirstAsync(u => u.Id == id); ;
            if (zppUser == null)
            {
                return HttpNotFound();
            }
            return View(zppUser);
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZppUser zppUser = await db.Users.FirstAsync(u => u.Id == id); ;
            if (zppUser == null)
            {
                return HttpNotFound();
            }
            return View(zppUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ZppUser zppUser = await db.Users.FirstAsync(u => u.Id == id); ;
            db.Users.Remove(zppUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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