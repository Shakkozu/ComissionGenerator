using ClassLibrary.Data;
using ClassLibrary.DataModels;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using System.IO;
using CommissionGeneratorMVC.Models;
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
            products.ForEach(x => commission.ProductPrices.Add(x.Cost));
            
            return View(commission);
        }

        // POST: Commission/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(CommissionMVCCreateModel commissionModel)
        {
            try
            {

                string zipFolderPath = $"{ Server.MapPath("~") }GeneratedZips";
                string docsFolderPath = $"{ Server.MapPath("~") }GeneratedDocuments";

                string documentsStorageFolder = Server.MapPath("~") + "GeneratedDocuments\\";
                string zipPath = $"{ Server.MapPath("~") }GeneratedZips\\result.zip";

                string uploadsFolderPath = $"{ Server.MapPath("~") }Uploads";
                string uploadsStorageFolder = $"{uploadsFolderPath}\\";

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }



                if (!Directory.Exists(docsFolderPath))
                {
                    Directory.CreateDirectory(docsFolderPath);
                }
                if (!Directory.Exists(zipFolderPath))
                {
                    Directory.CreateDirectory(zipFolderPath);
                }

                // Get Company information
                CompanyMVCModel documentCompany = GetCompanyInformation(commissionModel.CreationCompany, commissionModel.SelectedCompany);

                //Get Clients information
                List<ClientMVCModel> documentClients = GetClientsInformation(commissionModel.CreationClient, commissionModel.SelectedClients);


                //Get Products information
                List<ItemMVCModel> documentProducts = GetProductsInformation(commissionModel.SelectedProducts, commissionModel.ProductPrices, commissionModel.ProductQuantities);


                //Get Creator information
                CreatorMVCModel documentCreator = GetCreatorInformation(commissionModel.CommissionCreator, commissionModel.SelectedCreator);

                if(documentCompany == null || documentClients.Count == 0 || documentProducts.Count == 0 || documentCreator == null)
                {
                    return RedirectToAction("Create", "Commission");
                }


                string fileName;
                // Saving uploaded file
                if(commissionModel.PostedFile != null)
                {
                    fileName = Path.GetFileName(commissionModel.PostedFile.FileName);
                    commissionModel.PostedFile.SaveAs(Server.MapPath("~") + "Uploads\\" + fileName);
                }

                if (documentClients.Count > 1)
                {

                    foreach (var item in documentClients)
                    {
                        PersonalData personalData = new PersonalData(documentCompany.ConvertToCompanyModel(), item.ConvertToClientModel(), documentCreator.ConvertToCommissionCreatorModel());

                        string path = $"{documentsStorageFolder}{ documentCompany.CompanyName }_{ item.FullName.Replace("\"", "") }_{ DateTime.Today.ToShortDateString() }.docx";
                        path.Trim();
                        if (commissionModel.PostedFile != null)
                        {
                            DocumentHelper.GenerateDocumentFromTemplate($"{Server.MapPath("~")}\\Uploads\\{Path.GetFileName(commissionModel.PostedFile.FileName)}",
                                path, personalData, documentProducts.ConvertToItemModel(),true);
                        }
                        else
                        {
                            DocumentHelper.GenerateNewDocument(path,
                                                            personalData, documentProducts.ConvertToItemModel());
                        }
                    }

                    ZipFile.CreateFromDirectory(docsFolderPath, zipPath);

                    // Append headers
                    Response.AppendHeader("content-disposition", $"attachment; filename={ documentCompany.CompanyName }_{ DateTime.Today.ToShortDateString() }.zip");
                    // Open/Save dialog
                    Response.ContentType = "application/octet-stream";
                    // Push it!
                    Response.TransmitFile(zipPath);
                    Response.End();

                }
                else
                {
                    PersonalData personalData = new PersonalData(documentCompany.ConvertToCompanyModel(), documentClients.First().ConvertToClientModel(), documentCreator.ConvertToCommissionCreatorModel());
                    string path = $"{documentsStorageFolder}{ documentCompany.CompanyName }_{ documentClients.First().FullName.Replace("\"", "") }_{ DateTime.Today.ToShortDateString() }.docx";

                     path.Trim();
                    if (commissionModel.PostedFile != null)
                    {
                        DocumentHelper.GenerateDocumentFromTemplate(uploadsStorageFolder + Path.GetFileName(commissionModel.PostedFile.FileName),
                            path, personalData, documentProducts.ConvertToItemModel(), true);
                    }
                    else
                    {
                        DocumentHelper.GenerateNewDocument(path,
                            personalData, documentProducts.ConvertToItemModel());
                    }
                    // Append headers
                    Response.AppendHeader("content-disposition", $"attachment; filename={ documentCompany.CompanyName }_{ documentClients.First().FullName.Replace("\"", "") }_{ DateTime.Today.ToShortDateString() }.docx");
                    // Open/Save dialog
                    Response.ContentType = "application/octet-stream";
                    // Push it!
                    Response.TransmitFile(path);
                    Response.End();
                }



                DeleteFiles();
                



                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                DeleteFiles();
                string msg = e.Message;

                return RedirectToAction("Create", "Commission");
            }
        }

        private CreatorMVCModel GetCreatorInformation(CreatorMVCModel commissionCreator, int selectedCreator)
        {
            CreatorMVCModel result = new CreatorMVCModel();
            List<CreatorMVCModel> creators = SQLiteDataAccess.LoadCreators();

            // Gets created creator information, and save him to DB
            if (commissionCreator.Name != null && commissionCreator.Name.Length > 0)
            {
                SQLiteDataAccess.SaveCreator(commissionCreator);
                result = commissionCreator;
            }
            // Load selected creator
            else
            {
                result = creators.Where(x => x.Id == selectedCreator).FirstOrDefault();
            }
            return result;
        }

        private List<ItemMVCModel> GetProductsInformation(List<int> selectedProducts, List<decimal> productPrices, List<int> productQuantities)
        {
            List<ItemMVCModel> products = SQLiteDataAccess.LoadProducts();
            List<ItemMVCModel> result = new List<ItemMVCModel>();
            foreach (int id in selectedProducts)
            {
                ItemMVCModel product = products.Where(x => x.Id == id).FirstOrDefault();

                product.Quantity = productQuantities[products.IndexOf(products.Where(x => x.Id == id).FirstOrDefault())];

                //If product price loaded from application differs from price loaded from DB, change the price for this document
                if (productPrices[products.IndexOf(products.Where(x => x.Id == id).FirstOrDefault())] != products.Where(x => x.Id == id).First().Cost)
                {
                    product.Cost = productPrices[products.IndexOf(products.Where(x => x.Id == id).FirstOrDefault())];
                }

                result.Add(product);
            }
            return result;
        }

        private List<ClientMVCModel> GetClientsInformation(ClientMVCModel creationClient, List<int> selectedClients)
        {
            List<ClientMVCModel> clients = SQLiteDataAccess.LoadMVCClients();
            List<ClientMVCModel> result = new List<ClientMVCModel>();

            if (creationClient.PhoneNumber != null)
            {
                SQLiteDataAccess.SaveClient(creationClient);
                if (creationClient.Company == false)
                {
                    creationClient.NIP = "";
                    creationClient.CompanyName = "";
                }
                result.Add(creationClient);
            }
            foreach (int id in selectedClients)
            {
                result.Add(clients.Where(x => x.Id == id).FirstOrDefault());
            }
            return result;
        }

        private CompanyMVCModel GetCompanyInformation(CompanyMVCModel creationCompany, int selectedCompany)
        {
            CompanyMVCModel result = new CompanyMVCModel();
            if (creationCompany.CompanyName != null)
            {
                SQLiteDataAccess.SaveCompany(creationCompany);
                result = creationCompany;
            }
            else
            {
                result = SQLiteDataAccess.LoadCompanies().Where(x => x.Id == selectedCompany).FirstOrDefault();
            }
            return result;
        }

        private void DeleteFiles()
        {
            //Remove generated Files from directory
            foreach (var filePath in Directory.EnumerateFiles($"{ Server.MapPath("~") }GeneratedDocuments"))
            {
                System.IO.File.Delete(filePath);
            }
            foreach (var filePath in Directory.EnumerateFiles($"{ Server.MapPath("~") }Uploads"))
            {
                System.IO.File.Delete(filePath);
            }
            //Remove generated ZIP
            foreach (var filePath in Directory.EnumerateFiles($"{ Server.MapPath("~") }GeneratedZips"))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
