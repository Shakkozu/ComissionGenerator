
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary 
{ 
    public class ClientModel : CommissionCreatorModel
    {
        #region Properties

        public AddressModel Address { get; set; } = new AddressModel();

        public bool Company { get; set; } = false;
       
        public string CompanyName { get; set; }
        public NIPModel NIP { get; set; } = new NIPModel();


        #endregion

        //***********************

        #region Constructors



        #endregion

        //***********************

        #region Methods

        public override string ToString()
        {
            string header;
            string footer="";
            if(Company)
            {
                header = $"{CompanyName}";
                footer = $"{NIP}";
            }
            else
            {
                header = $"{Name} {LastName}";
                
            }
            return $"{header}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{footer}";
        }


        #endregion





    }
}
