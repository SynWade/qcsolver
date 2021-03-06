﻿using System;
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
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (user.PersonType.type == "master")
                {
                    var companies = db.Companies.Include(c => c.Country1).Include(c => c.Province1);
                    return View(companies.ToList());
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

        // GET: Companies/Details/5
        public ActionResult Details()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["company"] != null && (user.PersonType.type == "master" || (user.PersonType.type == "admin" && Request["company"].ToString() == user.company.ToString())))
                {
                    var id = Request["company"].ToString();
                    var company = db.Companies.Include(c => c.Country1).Include(c => c.Province1).Where(c => c.companyId.ToString() == id).First();
                    return View(company);
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

        // GET: Companies/Create
        public ActionResult Create()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (user.PersonType.type == "master")
                {
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
        public ActionResult Edit()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["company"] != null && (user.PersonType.type == "master" || (user.PersonType.type == "admin" && Request["company"].ToString() == user.company.ToString())))
                {
                    string companyId = Request["company"].ToString();
                    var company = db.Companies.Where(c => c.companyId.ToString() == companyId).First();
                    if (company == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", company.country);
                    ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", company.province);
                    return View(company);
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

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
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
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["company"] != null && user.PersonType.type == "master")
                {
                    string companyId = Request["company"].ToString();
                    var company = db.Companies.Where(c => c.companyId.ToString() == companyId).First();
                    if (company == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.country = new SelectList(db.Countries, "countryId", "countryName", company.country);
                    ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName", company.province);
                    return View(company);
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
    }
}
