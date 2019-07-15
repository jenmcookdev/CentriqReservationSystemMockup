using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PawsNClaws.DATA.EF;

namespace PawsNClaws.Controllers
{
    public class ServicesProvidedsController : Controller
    {
        private PawsNClawsEntities db = new PawsNClawsEntities();

        // GET: ServicesProvideds
        public ActionResult Index()
        {
            return View(db.ServicesProvideds.ToList());
        }

        // GET: ServicesProvideds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesProvided servicesProvided = db.ServicesProvideds.Find(id);
            if (servicesProvided == null)
            {
                return HttpNotFound();
            }
            return View(servicesProvided);
        }

        // GET: ServicesProvideds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServicesProvideds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServicesProvidedID,ServicesProvided1")] ServicesProvided servicesProvided)
        {
            if (ModelState.IsValid)
            {
                db.ServicesProvideds.Add(servicesProvided);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicesProvided);
        }

        // GET: ServicesProvideds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesProvided servicesProvided = db.ServicesProvideds.Find(id);
            if (servicesProvided == null)
            {
                return HttpNotFound();
            }
            return View(servicesProvided);
        }

        // POST: ServicesProvideds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServicesProvidedID,ServicesProvided1")] ServicesProvided servicesProvided)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicesProvided).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicesProvided);
        }

        // GET: ServicesProvideds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesProvided servicesProvided = db.ServicesProvideds.Find(id);
            if (servicesProvided == null)
            {
                return HttpNotFound();
            }
            return View(servicesProvided);
        }

        // POST: ServicesProvideds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicesProvided servicesProvided = db.ServicesProvideds.Find(id);
            db.ServicesProvideds.Remove(servicesProvided);
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
