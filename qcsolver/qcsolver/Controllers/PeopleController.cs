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
    public class PeopleController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: People
        public ActionResult Index()
        {
            var people = db.People.Include(p => p.Company1).Include(p => p.Country1).Include(p => p.Province1).Include(p => p.PersonType);
            return View(people.ToList());
        }

        public ActionResult Login()
        {
            return View();
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
            ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "personId,firstName,lastName,city,address,pictureLocation,contractLocation,postalCode,phone,email,online,type,company,country,province")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", person.company);
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", person.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", person.province);
            ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type", person.type);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", person.company);
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", person.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", person.province);
            ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type", person.type);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "personId,firstName,lastName,city,address,pictureLocation,contractLocation,postalCode,phone,email,online,type,company,country,province")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", person.company);
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", person.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", person.province);
            ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type", person.type);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
