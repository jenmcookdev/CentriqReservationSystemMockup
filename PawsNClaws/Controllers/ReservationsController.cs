using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PawsNClaws.DATA.EF;
using Microsoft.AspNet.Identity;

namespace PawsNClaws.Controllers
{
    public class ReservationsController : Controller
    {
        private PawsNClawsEntities db = new PawsNClawsEntities();

        // GET: Reservations
        [Authorize]
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var today = DateTime.Now.AddDays(-1);

            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var reservations = db.Reservations.Include(r => r.Location).
                    Include(r => r.ServicesProvided).
                    Include(r => r.OwnerAsset).
                    Where(r => r.ReservationDate >= today).
                    OrderBy(r => r.ReservationDate);
                return View(reservations.ToList());
            }
            if (User.IsInRole("User"))
            {
                var reservations = db.Reservations.Include(r => r.Location).
                    Include(r => r.ServicesProvided).Include(r => r.OwnerAsset).
                    Where(r => r.OwnerAsset.OwnerID == user && r.ReservationDate >= today && r.OwnerAsset.IsActive == true).
                    OrderBy(r => r.ReservationDate);
                return View(reservations.ToList());
            }

            return View("Error");
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            ViewBag.ServicesProvidedID = new SelectList(db.ServicesProvideds, "ServicesProvidedID", "ServicesProvided1");

            var owner = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetId", "AssetName");
            }
            else
            {
                ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets.Where(x => x.OwnerID == owner && x.IsActive == true),
                "OwnerAssetID", "AssetName");
            }

            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,LocationID,ReservationDate,OwnerAssetID,ServicesProvidedID,Notes")]
        Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                    var location = db.Locations.Where(abo => abo.LocationID == reservation.LocationID).Single();
                    if ((location.ReservationLimit > location.Reservations.Where(x => x.ReservationDate == reservation.ReservationDate).Count()) || User.IsInRole("Admin"))
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                return View("ErrorPageResLimitExceeded");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            ViewBag.ServicesProvidedID = new SelectList(db.ServicesProvideds, "ServicesProvidedID", "ServicesProvided1", reservation.ServicesProvidedID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetName", reservation.OwnerAssetID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            ViewBag.ServicesProvidedID = new SelectList(db.ServicesProvideds, "ServicesProvidedID", "ServicesProvided1", reservation.ServicesProvidedID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetName", reservation.OwnerAssetID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,LocationID,ReservationDate,OwnerAssetID,ServicesProvidedID,Notes")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            ViewBag.ServicesProvidedID = new SelectList(db.ServicesProvideds, "ServicesProvidedID", "ServicesProvided1", reservation.ServicesProvidedID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetName", reservation.OwnerAssetID);
            return View(reservation);
        }

        [Authorize(Roles = "Employee, User")]
        public ActionResult EditNotes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNotes([Bind(Include = "ReservationID,LocationID,ReservationDate,OwnerAssetID,ServicesProvidedID,Notes")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            ViewBag.ServicesProvidedID = new SelectList(db.ServicesProvideds, "ServicesProvidedID", "ServicesProvided1", reservation.ServicesProvidedID);
            ViewBag.OwnerAssetID = new SelectList(db.OwnerAssets, "OwnerAssetID", "AssetName", reservation.OwnerAssetID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
