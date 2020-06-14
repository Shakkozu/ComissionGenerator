
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary 
{ 
    public class PersonModel
    {
        #region Properties


        public string Name { get; set; }
        public string Surname { get; set; }

        public AddressModel Address { get; set; } = new AddressModel();
        public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel("");

        #endregion

        //***********************

        #region Constructors



        #endregion

        //***********************

        #region Methods



        #endregion





    }
}
