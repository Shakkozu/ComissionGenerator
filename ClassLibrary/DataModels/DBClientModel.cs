using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.DataModels
{
    class DBClientModel
    {
        public int id { get; set; }
        public string NIP { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        
    }
}
