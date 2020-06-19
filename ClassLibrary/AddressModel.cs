using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AddressModel
    {
        #region Properties

        public PostalCodeModel PostalCode { get; set; } = new PostalCodeModel();
        public string Street { get; set; } 
        public string City { get; set; }


        #endregion

        //***********************

        #region Constructors

        public AddressModel()
        {
            City = "";
            Street = "";
        }

        public AddressModel(AddressModel model)
        {
            PostalCode = model.PostalCode;
            Street = model.Street;
            City = model.City;
        }

        #endregion

        //***********************

        #region Methods


        public override string ToString()
        {
            return $"ul. {Street}" +
                Environment.NewLine +
                $"{City}, {PostalCode}";
        }

        #endregion




    }
}
