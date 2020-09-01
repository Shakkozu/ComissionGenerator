using ClassLibrary.Data;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CommissionGeneratorMVC.Models;

namespace CommissionGeneratorMVC.Controllers
{
    public class CompaniesController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {

            List<CompanyModel> companies = SQLiteDataAccess.LoadCompanies();
            List<CompanyMVCModel> convertedCompanies = ConvertCompaniesToMVCModels(companies);

            return View(convertedCompanies);
        }

        private List<CompanyMVCModel> ConvertCompaniesToMVCModels(List<CompanyModel> companies)
        {
            List<CompanyMVCModel> output = new List<CompanyMVCModel>();
            foreach (CompanyModel company in companies)
            {
                output.Add(new CompanyMVCModel
                {
                    PostalCode = $"{ company.Address.PostalCode.Number }",
                    City = $"{ company.Address.City }",
                    Street = $"{ company.Address.Street }",
                    EmailAddress = $"{ company.EmailAddress.Address }",
                    PhoneNumber = $"{ company.PhoneNumber.Number }",
                    NIP = $"{ company.NIP.Number}",
                    REGON = $"{ company.REGON.Number }",
                    CompanyName = $"{ company.CompanyName}",
                    Id = company.Id
                });
            }
            return output;
        }

        private CompanyMVCModel ConvertCompanyToMVCModel(CompanyModel company)
        {
            CompanyMVCModel output = new CompanyMVCModel
            {
                PostalCode = $"{ company.Address.PostalCode.Number }",
                City = $"{ company.Address.City }",
                Street = $"{ company.Address.Street }",
                EmailAddress = $"{ company.EmailAddress.Address }",
                PhoneNumber = $"{ company.PhoneNumber.Number }",
                NIP = $"{ company.NIP.Number}",
                REGON = $"{ company.REGON.Number }",
                CompanyName = $"{ company.CompanyName}",
                Id = company.Id
            };
            return output;
        }

        

        // GET: Company/Create
        public ActionResult Create()
        {
            CompanyMVCModel input = new CompanyMVCModel();
            return View(input);
        }

        // POST: Company/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(CompanyMVCModel company)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    CompanyModel cpn = new CompanyModel(
                        company.Id, company.NIP, 
                        company.PostalCode, company.City,
                        company.Street, company.EmailAddress,
                        company.CompanyName, company.PhoneNumber,
                        company.REGON);
                    SQLiteDataAccess.SaveCompany(cpn);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }


        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            CompanyModel model = SQLiteDataAccess.LoadCompanies().Where(x => x.Id == id).FirstOrDefault();
           
            if (model != null)
            {
                CompanyMVCModel mvcModel = ConvertCompanyToMVCModel(model);
                return View(mvcModel);
            }
            return RedirectToAction("Index");
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CompanyMVCModel company)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    CompanyModel cpn = new CompanyModel(
                    company.Id, company.NIP,
                    company.PostalCode, company.City,
                    company.Street, company.EmailAddress,
                    company.CompanyName, company.PhoneNumber,
                    company.REGON);

                    SQLiteDataAccess.EditCompany(cpn);

                }
                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Edit",id);
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            CompanyModel model = SQLiteDataAccess.LoadCompanies().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                CompanyMVCModel mvcModel = ConvertCompanyToMVCModel(model);
                return View(mvcModel);
            }
            return RedirectToAction("Index");
        }

        // POST: Company/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CompanyMVCModel company)
        {
            try
            {
                if(company != null)
                {
                    SQLiteDataAccess.RemoveCompany(company.Id);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(company);
            }
        }
    }
}
