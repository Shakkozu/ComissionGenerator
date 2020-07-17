using System;

namespace ClassLibrary
{
    public class AddressModel
    {
        #region Properties

        public PostalCodeModel PostalCode { get; set; } = new PostalCodeModel();
        public string Street { get; set; }
        public string City { get; set; }
        public bool IsValid { get
            {
                if (Street != "" && City != "" && PostalCode.IsValid)
                    return true;
                else
                    return false;
            }
        }
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
            return $"{Street}" +
                Environment.NewLine +
                $"{PostalCode}, {City}";
        }

        #endregion




    }
}
