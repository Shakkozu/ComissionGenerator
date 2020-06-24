using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Windows.UI.Composition.Scenes;
using Windows.UI.Popups;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Documents;
using Windows.Foundation.Metadata;

namespace ComissionGeneratorUWP.ViewModel
{
    
    [DataContract]
    public class CompanyViewModel : BaseViewModel

    {
        #region Properties
     
        [DataMember] public AddressModel Address { get; set; } = new AddressModel();
        [DataMember] public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();

        [DataMember] public CommissionCreatorModel Creator { get; set; } = new CommissionCreatorModel();
        [DataMember] public EmailAddressModel EmailAddress { get; set; } = new EmailAddressModel();


        [DataMember] public NIPModel NIP { get; set; } = new NIPModel();
        [DataMember] public RegonModel REGON { get; set; } = new RegonModel();

        [DataMember] public string CompanyName { get; set; }
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
            if (!Address.PostalCode.IsValid || !PhoneNumber.IsValid ||
                !Creator.PhoneNumber.IsValid || !NIP.IsValid || !REGON.IsValid)
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
                    OnPropertyChanged("Creator");
                }
                if (model.Address != null)
                {
                    if (model.Address.City != null) Address.City = model.Address.City;
                    if (model.Address.PostalCode.IsValid) Address.PostalCode = model.Address.PostalCode;
                    if (model.Address.Street != null) Address.Street = model.Address.Street;
                    OnPropertyChanged("Address");
                }
                if (model.CompanyName != null)
                {
                    CompanyName = model.CompanyName;
                    OnPropertyChanged("CompanyName");
                }
                if (model.NIP.IsValid)
                {
                    NIP = model.NIP;
                    OnPropertyChanged("NIP");
                }
                if (model.EmailAddress.IsValid)
                {
                    EmailAddress = model.EmailAddress;
                    OnPropertyChanged("EmailAddress");
                }
                if (model.PhoneNumber.IsValid)
                {
                    PhoneNumber = model.PhoneNumber;
                    OnPropertyChanged("PhoneNumber");
                }
                if (model.REGON.IsValid)
                {
                    REGON = model.REGON;
                    OnPropertyChanged("REGON");
                }
            }
         }

        /// <summary>
        /// Save current properties to file in xml format
        /// </summary>
        /// <returns></returns>
        async internal Task<bool> SaveXml()
        {
            return await base.Save(this, ExtensionType.Xml);
        }


        /// <summary>
        /// Load CompanyViewModel from File in xml format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> LoadXml()
        {
            return await base.Load(this, ExtensionType.Xml);
        }

        /// <summary>
        /// Load CompanyViewModel from File in json format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> LoadJson()
        {
            return await base.Load(this, ExtensionType.Json);
        }

        /// <summary>
        /// Save current properties to file in json format
        /// </summary>
        /// <returns></returns>
        async internal Task<bool> SaveJson()
        {
            return await base.Save(this, ExtensionType.Json);
        }



        #endregion
    }
}
