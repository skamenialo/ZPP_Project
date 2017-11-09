using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserType,Banned,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ZppUser zppUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(zppUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(zppUser);
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