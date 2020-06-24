using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ComissionGeneratorUWP.ViewModel
{
    [KnownType(typeof(ClientViewModel))]
    public class ClientViewModel : BaseViewModel// INotifyPropertyChanged
    {

        #region Properties

        [DataMember] public string Name { get; set; }
        [DataMember] public string LastName { get; set; }

        [DataMember] public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();
        [DataMember] public AddressModel Address { get; set; } = new AddressModel();

        #endregion

        #region Constructors

        public ClientViewModel():base("clientViewModel")
        {

        }

        #endregion

        

        #region Methods

        
        /// <summary>
        /// Check if any of properties is incorrect
        /// </summary>
        /// <returns>if all data is OK returns true, false otheriwse</returns>
        public bool CheckValidation()
        {
            if (!PhoneNumber.IsValid | !Address.PostalCode.IsValid)
            {
                return false;
            }
            return true;
        }

        

        /// <summary>
        /// Load Properties from parameter to current clientViewModel
        /// Function loads only valid data
        /// PS.Unfortunately overloading operator '=' is not possible so data overwriting needs to be written this way
        /// </summary>
        protected override void LoadProperties(object obj)
        {
            if (obj is ClientViewModel model)
            {
                if (model.Address != null)
                {
                    if (model.Address.City != null) Address.City = model.Address.City;
                    if (model.Address.PostalCode.IsValid) Address.PostalCode = model.Address.PostalCode;
                    if (model.Address.Street != null) Address.Street = model.Address.Street;
                    OnPropertyChanged("Address");
                }
                if (model.PhoneNumber.IsValid)
                { 
                    PhoneNumber = model.PhoneNumber;
                    OnPropertyChanged("PhoneNumber");
                }
                if (model.Name != null)
                {
                    Name = model.Name;
                    OnPropertyChanged("Name");
                }

                if (model.LastName != null)
                {
                    LastName = model.LastName;
                    OnPropertyChanged("LastName");
                }

            }
            
        }


        /// <summary>
        /// Save current properties to file in xml format
        /// </summary>
        /// <returns></returns>
        async internal Task<bool> SaveXml()
        {
            return await base.Save(this,ExtensionType.Xml);
        }



        /// <summary>
        /// Load clientViewModel from File in xml format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> LoadXml()
        {
            return await base.Load(this,ExtensionType.Xml);
        }

        /// <summary>
        /// Loads clientViewModel from File in json format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> LoadJson()
        {
            return await base.Load(this, ExtensionType.Json);
        }

        /// <summary>
        /// Saves clientViewModel to file in json format
        /// </summary>
        /// <returns>true if saving is succesful, false otherwise</returns>
        async internal Task<bool> SaveJson()
        {
            return await base.Save(this,ExtensionType.Json);
        }



        #endregion
    }
}
