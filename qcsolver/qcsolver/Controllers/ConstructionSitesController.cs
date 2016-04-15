using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qcsolver.Models;
using System.Threading.Tasks;

namespace qcsolver.Controllers
{
    public class ConstructionSitesController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: ConstructionSites
        public ActionResult Index()
        {
            var constructionSites = db.ConstructionSites.Include(c => c.Company1).Include(c => c.Country1).Include(c => c.Province1);
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["company"] != null)
                {
                    var company = Request["company"].ToString();
                    if ((company == user.company.ToString() && user.PersonType.type == "admin") || user.PersonType.type == "master")
                    {
                        constructionSites = constructionSites.Where(c => c.company.ToString() == company);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");

                    }
                }
                else if (user.PersonType.type != "master")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View(constructionSites.ToList());
        }

        // GET: ConstructionSites/Details/5
        public ActionResult Details()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["constructionSite"] != null && (user.PersonType.type == "master" || (user.PersonType.type == "admin" && db.ConstructionSites.Where(c => c.constructionSiteId.ToString() == Request["constructionSite"].ToString()).First().company.ToString() == user.company.ToString())))
                {
                    var id = Request["constructionSite"].ToString();
                    var constructionSite = db.ConstructionSites.Include(c => c.Country1).Include(c => c.Province1).Where(c => c.constructionSiteId.ToString() == id).First();
                    return View(constructionSite);
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
        }

        // GET: ConstructionSites/Create
        public ActionResult Create()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (user.PersonType.type == "master" || user.PersonType.type == "admin")
                {
                    ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
                    ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
                    ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
                    return View();
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
        public ActionResult Edit()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["constructionSite"] != null && (user.PersonType.type == "master" || (user.PersonType.type == "admin" && db.ConstructionSites.Where(c => c.constructionSiteId.ToString() == Request["constructionSite"].ToString()).First().company.ToString() == user.company.ToString())))
                {
                    var id = Request["constructionSite"].ToString();
                    var constructionSite = db.ConstructionSites.Include(c => c.Country1).Include(c => c.Province1).Where(c => c.constructionSiteId.ToString() == id).First();
                    ViewBag.company = new SelectList(db.Companies, "companyId", "companyName", constructionSite.company);
                    ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", constructionSite.country);
                    ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", constructionSite.province);
                    return View(constructionSite);
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



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult AssignWorker()
        {
            if (Session["user"] != null)
            {
                if (((Person)Session["user"]).PersonType.type != "subcontractor" && ((Person)Session["user"]).PersonType.type != "contractor")
                {
                    if (((Person)Session["user"]).PersonType.type == "master")
                    {
                        ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "address");
                        ViewBag.person = new SelectList(db.People, "personId", "firstName");
                    }
                    else
                    {
                        ViewBag.constructionSite = new SelectList(db.ConstructionSites.Where(x => x.company == ((Person)Session["user"]).company), "constructionSiteId", "address");
                        ViewBag.person = new SelectList(db.People.Where(x => x.company == ((Person)Session["user"]).company), "personId", "firstName");
                    }
                    return View();
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
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignWorker(AssignedWorker assigned)
        {
            if (ModelState.IsValid)
            {
                if (Session["user"] != null)
                {
                    if (((Person)Session["user"]).PersonType.type != "subcontractor" && ((Person)Session["user"]).PersonType.type != "contractor")
                    {
                        if (assigned.ConstructionSite1.company == assigned.Person1.company && (((Person)Session["user"]).PersonType.type == "master" || (((Person)Session["user"]).PersonType.personTypeId < assigned.Person1.PersonType.personTypeId && ((((Person)Session["user"]).PersonType.type == "admin" && ((Person)Session["user"]).company == assigned.ConstructionSite1.company) || (((Person)Session["user"]).PersonType.type == "supervisor" && ((Person)Session["user"]).AssignedWorkers.First().constructionSite == assigned.constructionSite)))))
                        {
                            db.AssignedWorkers.Add(assigned);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Account");
                        }
                        else
                        {
                            return View(assigned);
                        }
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
            }
            return View(assigned);
        }

        // GET: ConstructionSites/Delete/5
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["constructionSite"] != null && (user.PersonType.type == "master" || (user.PersonType.type == "admin" && db.ConstructionSites.Where(c => c.constructionSiteId.ToString() == Request["constructionSite"].ToString()).First().company.ToString() == user.company.ToString())))
                {
                    var id = Request["constructionSite"].ToString();
                    var constructionSite = db.ConstructionSites.Include(c => c.Country1).Include(c => c.Province1).Where(c => c.constructionSiteId.ToString() == id).First();
                    return View(constructionSite);
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
