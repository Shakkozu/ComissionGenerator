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
            List<ClientMVCModel> mvcClients = SQLiteDataAccess.LoadMVCClients();
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
        //TODO Fix validation, if company checkBox is checked, CompanyName and NIP are needed, and if it's not checked, FirstName and LastName are needed
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
            ClientMVCModel model = SQLiteDataAccess.LoadMVCClients().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: Clients/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ClientMVCModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    SQLiteDataAccess.EditClient(model);
                }
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
            ClientMVCModel model = SQLiteDataAccess.LoadMVCClients().Where(x => x.Id == id).FirstOrDefault();

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: Clients/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ClientMVCModel model)
        {
            try
            {
                if (model != null)
                {
                    SQLiteDataAccess.RemoveClient(model.Id);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


    }
}
