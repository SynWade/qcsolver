using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qcsolver.Models;

namespace qcsolver.Controllers
{
    public class TimestampsController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: Timestamps
        public ActionResult Index()
        {
            var timestamps = db.Timestamps.Include(t => t.ConstructionSite1).Include(t => t.Person1);
            return View(timestamps.ToList());
        }

        // GET: Timestamps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timestamp timestamp = db.Timestamps.Find(id);
            if (timestamp == null)
            {
                return HttpNotFound();
            }
            return View(timestamp);
        }

        // GET: Timestamps/Create
        public ActionResult Create()
        {
            ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "address");
            ViewBag.person = new SelectList(db.People, "personId", "firstName");
            return View();
        }

        // POST: Timestamps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "timestampId,timeIn,timeOut,person,constructionSite")] Timestamp timestamp)
        {
            if (ModelState.IsValid)
            {
                db.Timestamps.Add(timestamp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "address", timestamp.constructionSite);
            ViewBag.person = new SelectList(db.People, "personId", "firstName", timestamp.person);
            return View(timestamp);
        }

        // GET: Timestamps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timestamp timestamp = db.Timestamps.Find(id);
            if (timestamp == null)
            {
                return HttpNotFound();
            }
            ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "address", timestamp.constructionSite);
            ViewBag.person = new SelectList(db.People, "personId", "firstName", timestamp.person);
            return View(timestamp);
        }

        // POST: Timestamps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "timestampId,timeIn,timeOut,person,constructionSite")] Timestamp timestamp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timestamp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "address", timestamp.constructionSite);
            ViewBag.person = new SelectList(db.People, "personId", "firstName", timestamp.person);
            return View(timestamp);
        }

        // GET: Timestamps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timestamp timestamp = db.Timestamps.Find(id);
            if (timestamp == null)
            {
                return HttpNotFound();
            }
            return View(timestamp);
        }

        // POST: Timestamps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Timestamp timestamp = db.Timestamps.Find(id);
            db.Timestamps.Remove(timestamp);
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
