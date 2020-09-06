using ClassLibrary.Data;
using ClassLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommissionGeneratorMVC.Controllers
{
    public class ItemsController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            List<ItemMVCModel> items = SQLiteDataAccess.LoadProducts();
            return View(items);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ItemMVCModel model = new ItemMVCModel();
            return View(model);
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(ItemMVCModel model)
        {
            try
            {
                
                if(ModelState.IsValid)
                {
                    SQLiteDataAccess.SaveProduct(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            ItemMVCModel model = SQLiteDataAccess.LoadProducts().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ItemMVCModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SQLiteDataAccess.EditProduct(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            ItemMVCModel model = SQLiteDataAccess.LoadProducts().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ItemMVCModel model)
        {
            try
            {
                if(model != null)
                {
                    SQLiteDataAccess.RemoveProduct(model);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
