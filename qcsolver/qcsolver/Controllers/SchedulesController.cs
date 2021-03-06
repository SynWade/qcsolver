﻿using System;
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
    public class SchedulesController : Controller
    {
        private onsightdbEntities db = new onsightdbEntities();

        // GET: Schedules
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var person = db.People.Where(x => x.personId.ToString() == personId).First();
                    var schedules = db.Schedules.Where(c => c.person.ToString() == personId);
                    if (person.PersonType.type != "master" && person.PersonType.type != "admin" && (user.PersonType.type == "master" || user.PersonType.personTypeId < person.PersonType.personTypeId || user.personId == person.personId))
                    {
                        return View(schedules);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var schedules = db.Schedules.Where(c => c.person == user.personId);
                    return View(schedules);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Schedules/Details/5
        public ActionResult Details()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["schedule"] != null)
                {
                    var scheduleId = Request["schedule"].ToString();
                    if (user.PersonType.type == "master" || (user.PersonType.personTypeId < db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.PersonType.personTypeId && user.company == db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.company) || user.personId == db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.personId)
                    {
                        var schedule = db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).First();
                        ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "constructionSiteName", schedule.constructionSite);
                        ViewBag.person = new SelectList(db.People, "personId", "firstName", schedule.person);
                        return View(schedule);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
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

        // GET: Schedules/Create
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
                        ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "constructionSiteName");
                        ViewBag.person = new SelectList(db.People, "personId", "firstName");
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "constructionSiteName");
                    ViewBag.person = new SelectList(db.People, "personId", "firstName");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "constructionSiteName", schedule.constructionSite);
            ViewBag.person = new SelectList(db.People, "personId", "firstName", schedule.person);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["schedule"] != null)
                {
                    var scheduleId = Request["schedule"].ToString();
                    if (user.PersonType.type == "master" || (user.PersonType.personTypeId < db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.PersonType.personTypeId && user.company == db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.company) || user.personId == db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.personId)
                    {
                        var schedule = db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).First();
                        ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "constructionSiteName", schedule.constructionSite);
                        ViewBag.person = new SelectList(db.People, "personId", "firstName", schedule.person);
                        return View(schedule);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
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

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.constructionSite = new SelectList(db.ConstructionSites, "constructionSiteId", "constructionSiteName", schedule.constructionSite);
            ViewBag.person = new SelectList(db.People, "personId", "firstName", schedule.person);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["schedule"] != null)
                {
                    var scheduleId = Request["schedule"].ToString();
                    if (user.PersonType.type == "master" || (user.PersonType.personTypeId < db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.PersonType.personTypeId && user.company == db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.company) || user.personId == db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).FirstOrDefault().Person1.personId)
                    {
                        var schedule = db.Schedules.Where(c => c.scheduleId.ToString() == scheduleId).First();
                        return View(schedule);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
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

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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
