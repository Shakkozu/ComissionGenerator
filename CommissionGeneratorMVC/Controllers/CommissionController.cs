﻿using ClassLibrary.Data;
using ClassLibrary.DataModels;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary.Helpers;
using System.IO.Compression;
using System.IO;

namespace CommissionGeneratorMVC.Controllers
{
    public class CommissionController : Controller
    {

        // GET: Commission/Create
        public ActionResult Create()
        {
            CommissionMVCCreateModel commission = new CommissionMVCCreateModel();

            List<CompanyMVCModel> companies = SQLiteDataAccess.LoadCompanies();
            commission.Companies = companies.Select(x => new SelectListItem { Text = x.CompanyName, Value = x.Id.ToString() }).ToList();

            List<ClientMVCModel> clients = SQLiteDataAccess.LoadMVCClients();
            commission.Clients = clients.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() }).ToList();

            List<CreatorMVCModel> creators = SQLiteDataAccess.LoadCreators();
            commission.Creators = creators.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() }).ToList();


            List<ItemMVCModel> products = SQLiteDataAccess.LoadProducts();
            commission.Products = products.Select(x => new SelectListItem { Text = $"{ x.ItemName }, { x.Cost }zł / {x.ItemUnit} " , Value = x.Id.ToString() }).ToList();

            products.ForEach(x => commission.ProductQuantities.Add(1));
            products.ForEach(x => commission.ProductPrices.Add(0));
            
            return View(commission);
        }

        // POST: Commission/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(CommissionMVCCreateModel commissionModel)
        {
            try
            {
                
                CompanyMVCModel documentCompany = new CompanyMVCModel();
                CreatorMVCModel documentCreator = new CreatorMVCModel();
                List <ClientMVCModel> documentClients = new List<ClientMVCModel>();
                List<ItemMVCModel> documentsProducts = new List<ItemMVCModel>();

                List <ClientMVCModel> clients = SQLiteDataAccess.LoadMVCClients();
                List <ItemMVCModel> products = SQLiteDataAccess.LoadProducts();
                List<CreatorMVCModel> creators = SQLiteDataAccess.LoadCreators();
                
                // Get Company information
                if(commissionModel.CreationCompany.CompanyName != null)
                {
                    SQLiteDataAccess.SaveCompany(commissionModel.CreationCompany);
                    documentCompany = commissionModel.CreationCompany;
                }
                else
                {
                    documentCompany = SQLiteDataAccess.LoadCompanies().Where(x => x.Id == commissionModel.SelectedCompany).FirstOrDefault();
                }

                //Get Clients information
                if(commissionModel.CreationClient.PhoneNumber != null)
                {
                    SQLiteDataAccess.SaveClient(commissionModel.CreationClient);
                    if(commissionModel.CreationClient.Company == false)
                    {
                        commissionModel.CreationClient.NIP = "";
                        commissionModel.CreationClient.CompanyName = "";
                    }
                    documentClients.Add(commissionModel.CreationClient);
                }
                foreach (int id in commissionModel.SelectedClients)
                {
                    documentClients.Add(clients.Where(x => x.Id == id).FirstOrDefault());
                }

                //Get Products information
                foreach (int id in commissionModel.SelectedProducts)
                {
                    ItemMVCModel product = products.Where(x => x.Id == id).FirstOrDefault();
                    
                    product.Quantity = commissionModel.ProductQuantities[products.IndexOf(products.Where(x => x.Id == id).FirstOrDefault())];
                    if (commissionModel.ProductPrices[products.IndexOf(products.Where(x => x.Id == id).FirstOrDefault())] != 0)
                    {
                        product.Cost = commissionModel.ProductPrices[products.IndexOf(products.Where(x => x.Id == id).FirstOrDefault())];
                    }
                    
                    documentsProducts.Add(product);
                }

                //Get Creator information
                //From form
                if (commissionModel.CommissionCreator.Name != null && commissionModel.CommissionCreator.Name.Length > 0)
                {
                    SQLiteDataAccess.SaveCreator(commissionModel.CommissionCreator);
                    documentCreator = commissionModel.CommissionCreator;
                }
                //From selection
                else
                {
                    documentCreator = creators.Where(x => x.Id == commissionModel.SelectedCreator).FirstOrDefault();
                }


                //TODO Add template selection page
                if (documentClients.Count > 1)
                {

                    foreach (var item in documentClients)
                    {
                        PersonalData personalData = new PersonalData(documentCompany.ConvertToCompanyModel(), item.ConvertToClientModel(), documentCreator.ConvertToCommissionCreatorModel());

                        string path = $"{ Server.MapPath("~") }\\GeneratedDocuments\\{ documentCompany.CompanyName }_{ item.FullName.Replace("\"", "") }_{ DateTime.Today.ToShortDateString() }.docx";
                        path.Trim();
                        DocumentHelper.GenerateNewDocument(path,
                            personalData, documentsProducts.ConvertToItemModel());
                    }

                    string zipPath = $"{ Server.MapPath("~") }result.zip";
                    string folderPath = $"{ Server.MapPath("~") }\\GeneratedDocuments";
                    ZipFile.CreateFromDirectory(folderPath, zipPath);

                    // Append headers
                    Response.AppendHeader("content-disposition", $"attachment; filename={ documentCompany.CompanyName }_{ DateTime.Today.ToShortDateString() }.zip");
                    // Open/Save dialog
                    Response.ContentType = "application/octet-stream";
                    // Push it!
                    Response.TransmitFile(zipPath);

                }
                else
                {
                    PersonalData personalData = new PersonalData(documentCompany.ConvertToCompanyModel(), documentClients.First().ConvertToClientModel(), documentCreator.ConvertToCommissionCreatorModel());

                    string path = $"{ Server.MapPath("~") }{ documentCompany.CompanyName }_{ documentClients.First().FullName.Replace("\"", "") }_{ DateTime.Today.ToShortDateString() }.docx";
                    path.Trim();
                    DocumentHelper.GenerateNewDocument(path,
                        personalData, products.ConvertToItemModel());

                    // Append headers
                    Response.AppendHeader("content-disposition", $"attachment; filename={ documentCompany.CompanyName }_{ documentClients.First().FullName.Replace("\"", "") }_{ DateTime.Today.ToShortDateString() }.docx");
                    // Open/Save dialog
                    Response.ContentType = "application/octet-stream";
                    // Push it!
                    Response.TransmitFile(path);
                }

                Response.End();

                //Remove generated Files from directory
                foreach (var file in Directory.EnumerateFiles($"{ Server.MapPath("~") }\\GeneratedDocuments"))
                {
                    System.IO.File.Delete(file);
                }
                //Remove generated ZIP
                System.IO.File.Delete($"{ Server.MapPath("~") }result.zip");

                return RedirectToAction("Index","Home");
            }
            catch(Exception e)
            {
                string msg = e.Message;

                return RedirectToAction("Create", "Commission");
            }
        }

    }
}
