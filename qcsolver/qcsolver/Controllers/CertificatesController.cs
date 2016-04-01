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
    public class CertificatesController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: /Certificates/
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var certificates = db.Certificates.Where(c => c.person.ToString() == personId);
                    if (certificates != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > certificates.First().Person1.PersonType.personTypeId))
                    {
                        return View(certificates);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var certificates = db.Certificates.Where(c => c.person == user.personId);
                    return View(certificates);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Certificates/Details/5
        public ActionResult Details()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (Request["certificate"] != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > db.Certificates.Where(c => c.person.ToString() == personId).First().Person1.PersonType.personTypeId))
                    {
                        var certificateId = Request["certificate"].ToString();
                        var certificate = db.Certificates.Where(c => c.person.ToString() == personId).Where(c => c.certificateId.ToString() == certificateId).First();
                        return View(certificate);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (Request["certificate"] != null && (user.PersonType.personTypeId == db.Certificates.Where(c => c.certificateId.ToString() == Request["certificate"].ToString()).First().Person1.PersonType.personTypeId))
                    {
                        var certificateId = Request["certificate"].ToString();
                        var certificate = db.Certificates.Where(c => c.person == user.personId).Where(c => c.certificateId.ToString() == certificateId).First();
                        return View(certificate);
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

        // GET: /Certificates/Create
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

        // POST: /Certificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "certificateId,certificateName,dateIssued,fileLocation,person")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                db.Certificates.Add(certificate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.person = new SelectList(db.People, "personId", "firstName", certificate.person);
            return View(certificate);
        }

        // GET: /Certificates/Edit/5
        public ActionResult Edit()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (Request["certificate"] != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > db.Certificates.Where(c => c.person.ToString() == personId).First().Person1.PersonType.personTypeId))
                    {
                        var certificateId = Request["certificate"].ToString();
                        var certificate = db.Certificates.Where(c => c.person.ToString() == personId).Where(c => c.certificateId.ToString() == certificateId).First();
                        return View(certificate);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (Request["certificate"] != null && (user.PersonType.personTypeId == db.Certificates.Where(c => c.certificateId.ToString() == Request["certificate"].ToString()).First().Person1.PersonType.personTypeId))
                    {
                        var certificateId = Request["certificate"].ToString();
                        var certificate = db.Certificates.Where(c => c.person == user.personId).Where(c => c.certificateId.ToString() == certificateId).First();
                        return View(certificate);
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

        // POST: /Certificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "certificateId,certificateName,dateIssued,fileLocation,person")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.person = new SelectList(db.People, "personId", "firstName", certificate.person);
            return View(certificate);
        }

        // GET: /Certificates/Delete/5
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    if (Request["certificate"] != null && (user.PersonType.type == "master" || user.PersonType.personTypeId > db.Certificates.Where(c => c.person.ToString() == personId).First().Person1.PersonType.personTypeId))
                    {
                        var certificateId = Request["certificate"].ToString();
                        var certificate = db.Certificates.Where(c => c.person.ToString() == personId).Where(c => c.certificateId.ToString() == certificateId).First();
                        return View(certificate);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (Request["certificate"] != null && (user.PersonType.personTypeId == db.Certificates.Where(c => c.certificateId.ToString() == Request["certificate"].ToString()).First().Person1.PersonType.personTypeId))
                    {
                        var certificateId = Request["certificate"].ToString();
                        var certificate = db.Certificates.Where(c => c.person == user.personId).Where(c => c.certificateId.ToString() == certificateId).First();
                        return View(certificate);
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

        // POST: /Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Certificate certificate = db.Certificates.Find(id);
            db.Certificates.Remove(certificate);
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
