
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary 
{ 
    public class PersonModel
    {
        private AddressModel _address = new AddressModel();
        private PhoneNumberModel _phoneNumber = new PhoneNumberModel();

        public string Name { get; set; }
        public string Surname { get; set; }

        public AddressModel Address
        {
            get
            {
                return _address;  
            }
            set 
            {
                _address = value;
            }
        }
        public PhoneNumberModel PhoneNumber
        { 
            get 
            { 
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value; 
            }
        }


    }
}
