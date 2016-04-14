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
                    var person = db.People.Where(x => x.personId.ToString() == personId).First();
                    var certificates = db.Certificates.Where(c => c.person.ToString() == personId);
                    if (user.PersonType.type == "master" || user.PersonType.personTypeId < person.PersonType.personTypeId || user.personId == person.personId)
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
                ViewBag.person = new SelectList(db.People, "personId", "firstName");
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
                ViewBag.person = new SelectList(db.People, "personId", "firstName");
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
        public ActionResult Create([Bind(Include = "certificateId,certificateName,dateIssued,fileLocation,person")] Certificate certificate, HttpPostedFileBase certificateFile)
        {
            if (ModelState.IsValid)
            {
                if (certificateFile != null)
                {
                    //If filename is too long, shorten it
                    var fileName = Path.GetFileName(certificateFile.FileName);
                    if (fileName.Length > 60)
                    {
                        fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                    }

                    var path = Path.Combine(Server.MapPath("~/Content/Users/" + certificate.Person1.email + "/Certificates"), fileName);
                    certificateFile.SaveAs(path);
                    certificate.fileLocation = fileName;
                }
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
                ViewBag.person = new SelectList(db.People, "personId", "firstName");
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
        public ActionResult Edit([Bind(Include = "certificateId,certificateName,dateIssued,fileLocation,person")] Certificate certificate, HttpPostedFileBase certificateFile)
        {
            if (ModelState.IsValid)
            {
                //Saves image files, then saves their path in database
                if (certificateFile != null)
                {
                    var fileName = Path.GetFileName(certificateFile.FileName);
                    if (fileName.Length > 60)
                    {
                        fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                    }

                    System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Users/" + certificate.Person1.email + "/Certificates");

                    foreach (FileInfo file in myDirInfo.GetFiles())
                    {
                        if(file.FullName == certificate.fileLocation)
                            file.Delete();
                    }
                    var path = Path.Combine(Server.MapPath("~/Content/Users/" + certificate.Person1.email + "/Certificates"), fileName);
                    certificateFile.SaveAs(path);
                    certificate.fileLocation = fileName;
                }
                db.Entry(certificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.person = new SelectList(db.People, "personId", "firstName", certificate.person);
            return View(certificate);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult ViewCertificate()
        {

            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null && Request["certificate"] != null)
                {
                    var personId = Request["person"].ToString();
                    var certificateId = Request["certificate"].ToString();
                    var certificate = db.Certificates.Where(c => c.person.ToString() == personId && c.certificateId.ToString() == certificateId).First();
                    if (certificate != null && (user.PersonType.type == "master" || certificate.Person1.PersonType.personTypeId > user.PersonType.personTypeId))
                    {
                        Response.ContentType = "Application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + certificate.fileLocation);
                        Response.TransmitFile(Server.MapPath("~/Content/Users/" + certificate.Person1.email + "/Certificates/" + certificate.fileLocation));
                        Response.End();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (Request["certificate"] != null)
                {
                    var certificateId = Request["certificate"].ToString();
                    var certificate = db.Certificates.Where(c => c.person.ToString() == user.personId.ToString() && c.certificateId.ToString() == certificateId).First();
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + certificate.fileLocation);
                    Response.TransmitFile(Server.MapPath("~/Content/Users/" + certificate.Person1.email + "/Certificates/" + certificate.fileLocation));
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

        // GET: /Certificates/Delete/5
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                ViewBag.person = new SelectList(db.People, "personId", "firstName");
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
            System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Users/" + certificate.Person1.email + "/Certificates");

            foreach (FileInfo file in myDirInfo.GetFiles())
            {
                if (file.FullName == certificate.fileLocation)
                    file.Delete();
            }
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
