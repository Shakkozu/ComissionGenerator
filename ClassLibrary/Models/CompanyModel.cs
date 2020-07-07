using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClassLibrary.Models
{
    public class CompanyModel
    {
         public EmailAddressModel EmailAddress { get; set; } = new EmailAddressModel();
         public AddressModel Address { get; set; } = new AddressModel();
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
