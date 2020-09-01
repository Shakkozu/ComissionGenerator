using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.DataModels
{
    public class ClientMVCModel
    {
        public int Id { get; set; }
        public string NIP { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        public string FullName { get
            {
                if(CompanyName != null && CompanyName.Length > 0)
                {
                    return CompanyName;
                }
                else
                {
                    return $"{ Name } { LastName }";
                }
            } }

        
    }
}
