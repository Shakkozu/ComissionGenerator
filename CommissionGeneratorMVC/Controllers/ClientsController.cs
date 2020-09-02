using ClassLibrary;
using ClassLibrary.Data;
using ClassLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommissionGeneratorMVC.Controllers
{
    public class ClientsController : Controller
    {
        // GET: Clients
        public ActionResult Index()
        {
            List<ClientModel> clients = SQLiteDataAccess.LoadClients();
            List<ClientMVCModel> mvcClients = ConvertClientsToMVCModels(clients);
            return View(mvcClients);
        }

        private List<ClientMVCModel> ConvertClientsToMVCModels(List<ClientModel> clients)
        {
            List<ClientMVCModel> output = new List<ClientMVCModel>();
            foreach (ClientModel client in clients)
            {
                output.Add(new ClientMVCModel
                {
                    PostalCode = $"{ client.Address.PostalCode.Number }",
                    City = $"{ client.Address.City }",
                    Street = $"{ client.Address.Street }",
                    EmailAddress = $"{ client.EmailAddress.Address }",
                    PhoneNumber = $"{ client.PhoneNumber.Number }",
                    NIP = $"{ client.NIP.Number}",
                    CompanyName = $"{ client.CompanyName}",
                    Id = client.Id,
                    Name = client.Name,
                    LastName = client.LastName
                });
            }
            return output;
        }

        private ClientMVCModel ConvertClientToMVCModel(ClientModel company)
        {
            ClientMVCModel output = new ClientMVCModel
            {
                PostalCode = $"{ company.Address.PostalCode.Number }",
                City = $"{ company.Address.City }",
                Street = $"{ company.Address.Street }",
                EmailAddress = $"{ company.EmailAddress.Address }",
                PhoneNumber = $"{ company.PhoneNumber.Number }",
                NIP = $"{ company.NIP.Number}",
                CompanyName = $"{ company.CompanyName}",
                Id = company.Id
            };
            return output;
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ClientMVCModel client = new ClientMVCModel();
            return View(client);
        }

        // POST: Clients/Create
        [HttpPost]
        public ActionResult Create(ClientMVCModel client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SQLiteDataAccess.SaveClient(client);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Clients/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Clients/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public JsonResult VerifyIfCompanyNameIsValid(string companyName, bool company)
       {
            if(company == false)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(companyName != null && companyName.Length > 1)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false,JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public JsonResult VerifyIfCompany(bool company, string companyName, string nip)
        {
            if (company == false)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (companyName != null && nip != null && companyName.Length > 0 && nip.Length > 0 )
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false,JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public JsonResult VerifyIfCompanyNIP(string nip, bool company)
        {
            if (company == false)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (nip != null && nip.Length > 0)
                {

                    return Json(NIPModel.Validate(nip), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
