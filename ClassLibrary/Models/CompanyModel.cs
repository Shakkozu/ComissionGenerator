using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClassLibrary.Models
{
    public class CompanyModel
    {
        [DataMember] public EmailAddressModel EmailAddress { get; set; } = new EmailAddressModel();
        [DataMember] public AddressModel Address { get; set; } = new AddressModel();
        [DataMember] public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();


        [DataMember] public NIPModel NIP { get; set; } = new NIPModel();
        [DataMember] public RegonModel REGON { get; set; } = new RegonModel();

        [DataMember] public string CompanyName { get; set; }

        public override string ToString()
        {
            return $"{CompanyName}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{NIP}\n{REGON}";
        }
    }
}
