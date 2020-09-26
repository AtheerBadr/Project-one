using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LastProject.Models;

namespace LastProject.Controllers
{
    public class SupervisorsController : Controller
    {
        private DBModel db = new DBModel();

        // GET: Supervisors
        public ActionResult Index()
        {
            var supervisor = db.Supervisor.Include(s => s.University);
            return View(supervisor.ToList());
        }

        // GET: Supervisors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = db.Supervisor.Find(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // GET: Supervisors/Create
        public ActionResult Create()
        {
            ViewBag.UniversityID = new SelectList(db.University, "UniversityID", "UniversityName");
            return View();
        }

        // POST: Supervisors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupervisorID,SupervisorName,SupervisorEmail,SupervisorJob,SubervisorMob,UniversityID")] Supervisor supervisor)
        {
            if (ModelState.IsValid)
            {
                db.Supervisor.Add(supervisor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UniversityID = new SelectList(db.University, "UniversityID", "UniversityName", supervisor.UniversityID);
            return View(supervisor);
        }

        // GET: Supervisors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = db.Supervisor.Find(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            ViewBag.UniversityID = new SelectList(db.University, "UniversityID", "UniversityName", supervisor.UniversityID);
            return View(supervisor);
        }

        // POST: Supervisors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupervisorID,SupervisorName,SupervisorEmail,SupervisorJob,SubervisorMob,UniversityID")] Supervisor supervisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supervisor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UniversityID = new SelectList(db.University, "UniversityID", "UniversityName", supervisor.UniversityID);
            return View(supervisor);
        }

        // GET: Supervisors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = db.Supervisor.Find(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // POST: Supervisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supervisor supervisor = db.Supervisor.Find(id);
            db.Supervisor.Remove(supervisor);
            db.SaveChanges();
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
