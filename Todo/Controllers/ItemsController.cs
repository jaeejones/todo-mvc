using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Todo.Models;

namespace Todo.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index()
        {
            // ORDER BY ListID, DueDateTime
            // LINQ!!!!
            var items = from n in db.Items
                        orderby n.ListID, n.DueDateTime
                        select n;

            return View(items);
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.ListID = new SelectList(db.Lists, "ListID", "ListTitle");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,ItemName,ListID,DueDateTime,Details,IsDone")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ListID = new SelectList(db.Lists, "ListID", "ListTitle", item.ListID);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListID = new SelectList(db.Lists, "ListID", "ListTitle", item.ListID);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,ItemName,ListID,DueDateTime,Details,IsDone")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListID = new SelectList(db.Lists, "ListID", "ListTitle", item.ListID);
            return View(item);
        }


        public ActionResult ToggleDone(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            if (item.IsDone)
            {
                item.IsDone = false;
            }
            else
            {
                item.IsDone = true;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult MarkAllDone(bool done) // int is place of the number on the list(which on mine is chicken ? allows nulls| this the  code that is called when we check a box on the idem list
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // if there is no number the app tells the user that is was a bad request with no number. Null means nothing, no data available
            }
            // find the thing that has the id or has the key of whatever the item involves, this could return null

            foreach (var MarkAllDone in db.Items)
            {
                MarkAllDone.IsDone = true;
            }
            Item item = db.Items.Find(id);

            if (item == null) // if the item is null(there is no data) then the return Http..etc will run below this line
            {
                return HttpNotFound();

            }

            if (item.IsDone) // Item is found, this code will change the properties on the console, this code looks at the value and flips it.
            {
                item.IsDone = false; //if Item Isdone change to false
            }
            else
            {
                item.IsDone = true;
            }

            db.SaveChanges();  // will automatically set up changes in the database 

            return RedirectToAction("Index");// any changes made in the datebase will be refreshed and relfected in the Index(veiew)

        }


        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
