using ClassLibrary.Data;
using ClassLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommissionGeneratorMVC.Controllers
{
    public class CreatorsController : Controller
    {
        // GET: Creators
        public ActionResult Index()
        {
            List<CreatorMVCModel> mvcCreators = SQLiteDataAccess.LoadCreators();
            return View(mvcCreators);
        }

        // GET: Creators/Create
        public ActionResult Create()
        {
            CreatorMVCModel creator = new CreatorMVCModel();
            return View(creator);
        }

        // POST: Creators/Create
        [HttpPost]
        public ActionResult Create(CreatorMVCModel creator)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    SQLiteDataAccess.SaveCreator(creator);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Creators/Edit/5
        public ActionResult Edit(int id)
        {
            CreatorMVCModel model = SQLiteDataAccess.LoadCreators().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: Creators/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CreatorMVCModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    SQLiteDataAccess.EditCreator(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Creators/Delete/5
        public ActionResult Delete(int id)
        {
            CreatorMVCModel model = SQLiteDataAccess.LoadCreators().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: Creators/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CreatorMVCModel model)
        {
            try
            {
                if(model != null)
                {
                    SQLiteDataAccess.RemoveCreator(model);
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
