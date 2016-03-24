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
    public class ConstructionSitesController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: ConstructionSites
        public ActionResult Index()
        {
            var constructionSites = db.ConstructionSites.Include(c => c.Company1).Include(c => c.Country1).Include(c => c.Province1);
            return View(constructionSites.ToList());
        }

        // GET: ConstructionSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstructionSite constructionSite = db.ConstructionSites.Find(id);
            if (constructionSite == null)
            {
                return HttpNotFound();
            }
            return View(constructionSite);
        }

        // GET: ConstructionSites/Create
        public ActionResult Create()
        {
            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
            return View();
        }

        // POST: ConstructionSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "constructionSiteId,address,startDate,endDate,city,company,country,province")] ConstructionSite constructionSite)
        {
            if (ModelState.IsValid)
            {
                db.ConstructionSites.Add(constructionSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", constructionSite.company);
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", constructionSite.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", constructionSite.province);
            return View(constructionSite);
        }

        // GET: ConstructionSites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstructionSite constructionSite = db.ConstructionSites.Find(id);
            if (constructionSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", constructionSite.company);
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", constructionSite.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", constructionSite.province);
            return View(constructionSite);
        }

        // POST: ConstructionSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "constructionSiteId,address,startDate,endDate,city,company,country,province")] ConstructionSite constructionSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(constructionSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", constructionSite.company);
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", constructionSite.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", constructionSite.province);
            return View(constructionSite);
        }

        // GET: ConstructionSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstructionSite constructionSite = db.ConstructionSites.Find(id);
            if (constructionSite == null)
            {
                return HttpNotFound();
            }
            return View(constructionSite);
        }

        // POST: ConstructionSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConstructionSite constructionSite = db.ConstructionSites.Find(id);
            db.ConstructionSites.Remove(constructionSite);
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
