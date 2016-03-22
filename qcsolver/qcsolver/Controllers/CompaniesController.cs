using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qcsolver.Models;
using System.ComponentModel.DataAnnotations;

namespace qcsolver.Controllers
{
    public class CompaniesController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.Country1).Include(c => c.Province1);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "companyId,companyName,contactNumber,contactEmail,address,postalCode,city,country,province")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", company.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", company.province);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", company.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", company.province);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "companyId,companyName,contactNumber,contactEmail,address,postalCode,city,country,province")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", company.country);
            ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", company.province);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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

        public class EmailAnnotation : RegularExpressionAttribute
        {
            static EmailAnnotation()
            {
                DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAnnotation), typeof(RegularExpressionAttributeAdapter));
            }


            public EmailAnnotation()
                : base(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$") { }

            public override string FormatErrorMessage(string name)
            {
                return "E-mail is not valid";
            }
        }
    }
}
