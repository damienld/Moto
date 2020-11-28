using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Moto.Data;

namespace PMotoWeb.Controllers
{
    public class UsersController : Controller
    {
        private string key = System.Configuration.ConfigurationManager.AppSettings["encryptKey"];
        private IDal dal;
        public UsersController() : this(new Dal("name = cnnMotoDb"))
        {
        }
        public UsersController(IDal dalIoc)
        {
            dal = dalIoc;
        }
        /*
        // GET: Users
        public ActionResult Index()
        {
            return View(dal.Db.Users.ToList());
        }
        */
        // GET: Register/Create User
        public ActionResult Create()
        {
            return View();
        }
        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Email,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                dal.AddUser(user, key, false);
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                return RedirectToAction("Index","Home");
            }
            //Roles.
            return View(user);
        }

        
        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Email,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                dal.UpdateUser(user, key, false);
                return RedirectToAction("Index");
            }
            return View(user);
        }
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = dal.Db.Users.Find(id);
            user.Password = "";
            user.ConfirmPassword = "";
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password")] User userInfo, string returnUrl)
        {
            User userLoggedin = dal.GetUser(userInfo.Username, userInfo.Password, key);
            if (userLoggedin != null)
            {
                FormsAuthentication.SetAuthCookie(userLoggedin.Id.ToString(), false);
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/");
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("User.Username", "Username and/or password incorrect(s)");
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = dal.Db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = dal.Db.Users.Find(id);
            dal.Db.Users.Remove(user);
            dal.Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dal.Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
