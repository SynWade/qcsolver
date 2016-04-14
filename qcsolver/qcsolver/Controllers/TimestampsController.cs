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
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var person = db.People.Where(x => x.personId.ToString() == personId).First();
                    var timestamps = db.Timestamps.Where(c => c.person.ToString() == personId);
                    if (timestamps != null && (user.PersonType.type == "master" || user.PersonType.personTypeId < person.PersonType.personTypeId || user.personId == person.personId))
                    {
                        return View(timestamps);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var timestamps = db.Timestamps.Where(c => c.person == user.personId);
                    return View(timestamps);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
