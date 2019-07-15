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
using System.IO;

namespace PawsNClaws.Controllers
{
    public class OwnerAssetsController : Controller
    {
        private PawsNClawsEntities db = new PawsNClawsEntities();

        // GET: OwnerAssets
        [Authorize]
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                var accounts = db.OwnerAssets.ToList();
                return View(accounts);
            }
            var user = User.Identity.GetUserId();
            var accountByOwner = db.OwnerAssets.Where(abo => abo.OwnerID == user && abo.IsActive == true);
            return View(accountByOwner.ToList());
            
        }

        // GET: OwnerAssets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: OwnerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerAssetID,AssetName,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")]
        OwnerAsset ownerAsset, HttpPostedFileBase AssetPhoto)
        {
            if (ModelState.IsValid)
            {
                ownerAsset.OwnerID = User.Identity.GetUserId();

                //***********  USER INFO AND FILE IMAGE UPLOAD
                #region User Information and File/Image Upload
                //default image will be noImage.jpg if no image is provided
                string image = "noImage.jpg";

                //check that image upload contains valid image
                if (AssetPhoto != null)
                {

                    //yes
                    //reassign the fileName to the variable that represents the default img
                    image = AssetPhoto.FileName;

                    //create a variable and retrieve the extension from the image
                    string ext = image.Substring(image.LastIndexOf("."));

                    //create a list of valid file extensions - (whitelist)
                    string[] goodExts = new string[] { ".jpg", ".png", ".jpeg", ".gif" };

                    //check our extension against that list
                    if (goodExts.Contains(ext.ToLower()))
                    {
                        //as long as our extension is in that list
                        //rename the file to a unique file name and add the extension
                        image = Guid.NewGuid() + ext;
                        //save the new file to the website

                        AssetPhoto.SaveAs(Server.MapPath("~/Content/assets/images/UserImages/" + image));

                    }
                    //if an invalid extension is provided
                    else
                    {
                        //go back to the default page
                        image = "noImage.jpg";
                    }
                }

                //No Matter What add the image name to the database object
                ownerAsset.AssetPhoto = image;
                #endregion

                ownerAsset.DateAdded = DateTime.Now;
                ownerAsset.IsActive = true;
                db.OwnerAssets.Add(ownerAsset);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(ownerAsset);
        }

        // GET: OwnerAssets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerAssetID,AssetName,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")]
        OwnerAsset ownerAsset, HttpPostedFileBase AssetPhoto)
        {
            if (ModelState.IsValid)
            {
                ownerAsset.OwnerID = User.Identity.GetUserId();

                //******************** FILE IMAGE UPLOAD
                #region User Information and File/Image Upload

                //if image is valid
                if (AssetPhoto != null)
                {

                    //store the new file name
                    string image = AssetPhoto.FileName;

                    //extract the extension and save it in a variable
                    string ext = image.Substring(image.LastIndexOf("."));

                    //valid file extensions - (whitelist)
                    string[] goodExts = new string[] { ".jpg", ".png", ".jpeg", ".gif" };

                    //check our extension against that list
                    if (goodExts.Contains(ext.ToLower()))
                    {
                        //if it's good...guid it.
                        //rename the file to a unique file name and add the extension
                        image = Guid.NewGuid() + ext;
                        //save the new file to the website

                        AssetPhoto.SaveAs(Server.MapPath("~/Content/assets/images/UserImages/" + image));

                        //remove the original(previous) image from website (NOT default image)
                        if (ownerAsset.AssetPhoto != "noImage.jpg")
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/assets/images/UserImages/" +
                                ownerAsset.AssetPhoto));
                        }
                        //save to database
                        ownerAsset.AssetPhoto = image;

                    }
                }

                //if upload is null the HiddenFor() will save original file
                #endregion

                db.Entry(ownerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            db.OwnerAssets.Remove(ownerAsset);

            //remove associated image(keeping default)
            if (ownerAsset.AssetPhoto != "noImage.jpg" && ownerAsset.AssetPhoto != null)
            {
                System.IO.File.SetAttributes(Server.MapPath("~/Content/assets/images/UserImages/" +
                    ownerAsset.AssetPhoto), FileAttributes.Normal);
                System.IO.File.Delete(Server.MapPath("~/Content/assets/images/UserImages/" +
                    ownerAsset.AssetPhoto));
            }
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
