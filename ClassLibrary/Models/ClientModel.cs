
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace ClassLibrary 
{
    public class ClientModel : CommissionCreatorModel
    {
        #region Properties

        public AddressModel Address { get; set; } = new AddressModel();

        public bool Company { get; set; } = false;

        public string CompanyName { get; set; }
        public NIPModel NIP { get; set; } = new NIPModel();

        public override string FullName => Company ? CompanyName : $"{Name} {LastName}";
         

        #endregion

        //***********************

        #region Constructors



        #endregion

        //***********************

        #region Methods

        public override string ToString()
        {
            string footer = Company ? $"{NIP}" : "";
            return $"{FullName}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{footer}";
        }


        #endregion





    }
}
