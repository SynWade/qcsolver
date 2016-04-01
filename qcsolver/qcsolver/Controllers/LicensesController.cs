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
    public class LicensesController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: Licenses
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var licenses = db.Licenses.Where(c => c.person.ToString() == personId);
                    if (licenses != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > licenses.First().Person1.PersonType.personTypeId))
                    {
                        return View(licenses);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var licenses = db.Licenses.Where(c => c.person == user.personId);
                    return View(licenses);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Licenses/Details/5
        public ActionResult Details()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (Request["license"] != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > db.Licenses.Where(c => c.person.ToString() == personId).First().Person1.PersonType.personTypeId))
                    {
                        var licenseId = Request["license"].ToString();
                        var license = db.Licenses.Where(c => c.person.ToString() == personId).Where(c => c.licenseId.ToString() == licenseId).First();
                        return View(license);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (Request["license"] != null && (user.PersonType.personTypeId == db.Licenses.Where(c => c.licenseId.ToString() == Request["license"].ToString()).First().Person1.PersonType.personTypeId))
                    {
                        var licenseId = Request["license"].ToString();
                        var license = db.Licenses.Where(c => c.person == user.personId).Where(c => c.licenseId.ToString() == licenseId).First();
                        return View(license);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Licenses/Create
        public ActionResult Create()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (user.PersonType.type == "master" || user.PersonType.personTypeId > db.People.Where(c => c.personId.ToString() == personId).First().personId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Licenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "licenseId,licenseName,dateIssued,expirationDate,fileLocation,person")] License license)
        {
            if (ModelState.IsValid)
            {
                db.Licenses.Add(license);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.person = new SelectList(db.People, "personId", "firstName", license.person);
            return View(license);
        }

        // GET: Licenses/Edit/5
        public ActionResult Edit()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (Request["license"] != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > db.Licenses.Where(c => c.person.ToString() == personId).First().Person1.PersonType.personTypeId))
                    {
                        var licenseId = Request["license"].ToString();
                        var license = db.Licenses.Where(c => c.person.ToString() == personId).Where(c => c.licenseId.ToString() == licenseId).First();
                        return View(license);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (Request["license"] != null && (user.PersonType.personTypeId == db.Licenses.Where(c => c.licenseId.ToString() == Request["license"].ToString()).First().Person1.PersonType.personTypeId))
                    {
                        var licenseId = Request["license"].ToString();
                        var license = db.Licenses.Where(c => c.person == user.personId).Where(c => c.licenseId.ToString() == licenseId).First();
                        return View(license);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Licenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "licenseId,licenseName,dateIssued,expirationDate,fileLocation,person")] License license)
        {
            if (ModelState.IsValid)
            {
                db.Entry(license).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.person = new SelectList(db.People, "personId", "firstName", license.person);
            return View(license);
        }

        // GET: Licenses/Delete/5
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (Request["license"] != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > db.Licenses.Where(c => c.person.ToString() == personId).First().Person1.PersonType.personTypeId))
                    {
                        var licenseId = Request["license"].ToString();
                        var license = db.Licenses.Where(c => c.person.ToString() == personId).Where(c => c.licenseId.ToString() == licenseId).First();
                        return View(license);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (Request["license"] != null && (user.PersonType.personTypeId == db.Licenses.Where(c => c.licenseId.ToString() == Request["license"].ToString()).First().Person1.PersonType.personTypeId))
                    {
                        var licenseId = Request["license"].ToString();
                        var license = db.Licenses.Where(c => c.person == user.personId).Where(c => c.licenseId.ToString() == licenseId).First();
                        return View(license);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Licenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            License license = db.Licenses.Find(id);
            db.Licenses.Remove(license);
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
