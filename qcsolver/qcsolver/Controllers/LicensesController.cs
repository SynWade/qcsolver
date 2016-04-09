using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qcsolver.Models;
using System.IO;

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
                    if (user.PersonType.type == "master" || user.PersonType.personTypeId > licenses.First().Person1.PersonType.personTypeId)
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
        public ActionResult Create([Bind(Include = "licenseId,licenseName,dateIssued,expirationDate,fileLocation,person")] License license, HttpPostedFileBase licenseFile)
        {
            if (ModelState.IsValid)
            {
                if (licenseFile != null)
                {
                    //If filename is too long, shorten it
                    var fileName = Path.GetFileName(licenseFile.FileName);
                    if (fileName.Length > 60)
                    {
                        fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                    }

                    var path = Path.Combine(Server.MapPath("~/Content/Users/" + license.Person1.email + "/Licenses"), fileName);
                    licenseFile.SaveAs(path);
                    license.fileLocation = fileName;
                }
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
        public ActionResult Edit([Bind(Include = "licenseId,licenseName,dateIssued,expirationDate,fileLocation,person")] License license, HttpPostedFileBase licenseFile)
        {
            if (ModelState.IsValid)
            {                //Saves image files, then saves their path in database
                if (licenseFile != null)
                {
                    var fileName = Path.GetFileName(licenseFile.FileName);
                    if (fileName.Length > 60)
                    {
                        fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                    }

                    System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Users/" + license.Person1.email + "/Licenses");

                    foreach (FileInfo file in myDirInfo.GetFiles())
                    {
                        if (file.FullName == license.fileLocation)
                            file.Delete();
                    }
                    var path = Path.Combine(Server.MapPath("~/Content/Users/" + license.Person1.email + "/Licenses"), fileName);
                    licenseFile.SaveAs(path);
                    license.fileLocation = fileName;
                }
                db.Entry(license).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.person = new SelectList(db.People, "personId", "firstName", license.person);
            return View(license);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult ViewLicense()
        {

            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null && Request["license"] != null)
                {
                    var personId = Request["person"].ToString();
                    var licenseId = Request["license"].ToString();
                    var license = db.Licenses.Where(c => c.person.ToString() == personId && c.licenseId.ToString() == licenseId).First();
                    if (license != null && (user.PersonType.type == "master" || license.Person1.PersonType.personTypeId > user.PersonType.personTypeId))
                    {
                        Response.ContentType = "Application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + license.fileLocation);
                        Response.TransmitFile(Server.MapPath("~/Content/Users/" + license.Person1.email + "/Licenses/" + license.fileLocation));
                        Response.End();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (Request["license"] != null)
                {
                    var licenseId = Request["license"].ToString();
                    var license = db.Licenses.Where(c => c.person.ToString() == user.personId.ToString() && c.licenseId.ToString() == licenseId).First();
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + license.fileLocation);
                    Response.TransmitFile(Server.MapPath("~/Content/Users/" + license.Person1.email + "/Licenses/" + license.fileLocation));
                    Response.End();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Home");
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
            System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Users/" + license.Person1.email + "/Licenses");
            foreach (FileInfo file in myDirInfo.GetFiles())
            {
                if (file.FullName == license.fileLocation)
                    file.Delete();
            }
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
