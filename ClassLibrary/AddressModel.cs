using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AddressModel
    {
        public PostalCodeModel PostalCode { get; }
        public string Street { get; set; }
        public string City { get; set; }

        public AddressModel(PostalCodeModel postalCode, string street, string city)
        {
            PostalCode = postalCode;
            Street = street;
            City = city;
        }

        public AddressModel()
        {
            PostalCode = new PostalCodeModel();
        }
    }
}
