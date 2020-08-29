using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommissionGeneratorMVC.Models
{
    public class CompanyMVCModel
    {
        public string EmailAddress { get; set; }
        public AddressModel Address { get; set; } = new AddressModel();

        public PostalCodeModel PostalCode { get; set; } = new PostalCodeModel();
        public string Street { get; set; }
        public string City { get; set; }
        public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();


        public NIPModel NIP { get; set; } = new NIPModel();
        public RegonModel REGON { get; set; } = new RegonModel();

        public string CompanyName { get; set; }

        public override string ToString()
        {
            return $"{CompanyName}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{NIP}\n{REGON}";
        }
    }
}