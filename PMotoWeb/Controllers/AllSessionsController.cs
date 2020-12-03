using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moto.Data;

namespace PMotoWeb.Controllers
{
    public class AllSessionsController : Controller
    {
        private IDal dal;
        public AllSessionsController() : this(new Dal("name=cnnMotoDb"))
        {
        }
        public AllSessionsController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        // GET: AllSessions
        public ActionResult Index(/*int? selectedCategoryId*/)
        {
            return View(dal.getAllSessions(null));
        }

        // GET: AllSessions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = dal.Db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }
        /*
        // GET: AllSessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AllSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionId,SessionType,Date,Note,GroundTemperature,IsWet")] Session session)
        {
            if (ModelState.IsValid)
            {
                dal.Db.Sessions.Add(session);
                dal.Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }*/
        /*
        // GET: AllSessions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = dal.Db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: AllSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionId,SessionType,Date,Note,GroundTemperature,IsWet")] Session session)
        {
            if (ModelState.IsValid)
            {
                dal.Db.Entry(session).State = EntityState.Modified;
                dal.Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }*/
        /*
        // GET: AllSessions/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = dal.Db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: AllSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Session session = dal.Db.Sessions.Find(id);
            dal.Db.Sessions.Remove(session);
            dal.Db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
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
