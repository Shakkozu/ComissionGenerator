using ClassLibrary.Models;
using System.Runtime.Serialization;

namespace ClassLibrary
{

    [DataContract]
    public class CompanyViewModel : BaseViewModel

    {
        #region Properties


        [DataMember] public CommissionCreatorModel Creator { get; set; } = new CommissionCreatorModel();
        [DataMember] public CompanyModel Company { get; set; } = new CompanyModel();
        #endregion

        //**************************

        #region Constructors

        public CompanyViewModel() : base("companyViewModel")
        {

        }

        #endregion


        //**************************

        #region Methods

        /// <summary>
        /// Check if any of properties is incorrect
        /// </summary>
        /// <returns>if all data is OK returns true, false otheriwse</returns>
        public bool CheckValidation()
        {
            if (!Company.Address.PostalCode.IsValid || !Company.PhoneNumber.IsValid ||
                !Creator.PhoneNumber.IsValid || !Company.NIP.IsValid || !Company.REGON.IsValid)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Load Properties from parameter to current CompanyViewModel
        /// Function loads only valid data
        /// PS.Unfortunately overloading operator '=' is not possible so data overwriting needs to be written this way
        /// </summary>
        protected override void LoadProperties(object obj)
        {
            if (obj is CompanyViewModel model)
            {
                if (model.Creator != null)
                {
                    //I need to specify properties, because otherwise null 'sneaks in' into one of properties
                    //in case that field is empty
                    if (model.Creator.LastName != null)
                        Creator.LastName = model.Creator.LastName;
                    if (model.Creator.Name != null)
                        Creator.Name = model.Creator.Name;
                    if (model.Creator.PhoneNumber.IsValid)
                        Creator.PhoneNumber = model.Creator.PhoneNumber;
                    if (model.Creator.EmailAddress.IsValid)
                        Creator.EmailAddress = model.Creator.EmailAddress;
                    OnPropertyChanged("Creator");
                }
                if (model.Company.Address != null)
                {
                    if (model.Company.Address.City != null) Company.Address.City = model.Company.Address.City;
                    if (model.Company.Address.PostalCode.IsValid) Company.Address.PostalCode = model.Company.Address.PostalCode;
                    if (model.Company.Address.Street != null) Company.Address.Street = model.Company.Address.Street;
                    OnPropertyChanged("Address");
                }
                if (model.Company.CompanyName != null)
                {
                    Company.CompanyName = model.Company.CompanyName;
                    OnPropertyChanged("CompanyName");
                }
                if (model.Company.NIP.IsValid)
                {
                    Company.NIP = model.Company.NIP;
                    OnPropertyChanged("NIP");
                }
                if (model.Company.EmailAddress.IsValid)
                {
                    Company.EmailAddress = model.Company.EmailAddress;
                    OnPropertyChanged("EmailAddress");
                }
                if (model.Company.PhoneNumber.IsValid)
                {
                    Company.PhoneNumber = model.Company.PhoneNumber;
                    OnPropertyChanged("PhoneNumber");
                }
                if (model.Company.REGON.IsValid)
                {
                    Company.REGON = model.Company.REGON;
                    OnPropertyChanged("REGON");
                }
            }
        }

        /// <summary>
        /// Save current properties to file in xml format
        /// </summary>
        /// <returns></returns>
        public bool SaveXml()
        {
            return Save(this, ExtensionType.Xml);
        }


        /// <summary>
        /// Load CompanyViewModel from File in xml format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        public bool LoadXml()
        {
            return Load(this, ExtensionType.Xml);
        }

        /// <summary>
        /// Load CompanyViewModel from File in json format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        public bool LoadJson()
        {
            return Load(this, ExtensionType.Json);
        }

        /// <summary>
        /// Save current properties to file in json format
        /// </summary>
        /// <returns></returns>
        public bool SaveJson()
        {
            return Save(this, ExtensionType.Json);
        }



        #endregion
    }
}
