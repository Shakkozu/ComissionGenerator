using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClassLibrary.DataModels;

namespace CommissionGeneratorMVC.Models
{
    public class CommissionMVCCreateModel
    {
        public List<SelectListItem> Companies { get; set; }

        public int SelectedCompany { get; set; }

        public CompanyMVCModel CreationCompany { get; set; } = new CompanyMVCModel();


        [Display(Name = "Product")]
        public List<SelectListItem> Products { get; set; }

        public List<int> SelectedProducts { get; set; } = new List<int>();


        public List<int> SelectedClients { get; set; } = new List<int>();

        public ClientMVCModel CreationClient { get; set; } = new ClientMVCModel();

        public List<SelectListItem> Clients { get; set; }

        [Display(Name = "Product Quantity")]
        [Range(1,10000)]
        public List<int> ProductQuantities { get; set; } = new List<int>();

        [Display(Name = "Product Price")]
        [Range(0,1000000)]
        public List<decimal> ProductPrices { get; set; } = new List<decimal>();


        public List<SelectListItem> Creators { get; set; }
        public CreatorMVCModel CommissionCreator { get; set; }
        public int SelectedCreator { get; set; }

        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.docx|.doc|.odt)$", ErrorMessage = "Only doc files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }


    }
}
