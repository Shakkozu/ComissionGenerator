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

namespace ComissionGeneratorUWP
{
    
    [DataContract]
    public class CompanyViewModel : INotifyPropertyChanged //This class doens't inheritate from BindableBase, because it causes some trouble with Serialization

    {
        #region Properties
     
        [DataMember]public AddressModel Address { get; set; } = new AddressModel();
        [DataMember] public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();

        [DataMember] public CommissionCreatorModel Creator { get; set; } = new CommissionCreatorModel();
        [DataMember] public EmailAddressModel EmailAddress { get; set; } = new EmailAddressModel();


        [DataMember] public NIPModel NIP { get; set; } = new NIPModel();
        [DataMember] public RegonModel REGON { get; set; } = new RegonModel();

        [DataMember] public string CompanyName { get; set; }
        #endregion

        //**************************

        #region Constructors
        public CompanyViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;


        #endregion


        //**************************

        #region Methods

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        private void OnPropertyChanged(string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


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
        /// Save current properties to file
        /// </summary>
        /// <returns></returns>
        async internal Task<bool> Save()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(CompanyViewModel));
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile companyViewModelFile = await localFolder.CreateFileAsync("companyView.xml", CreationCollisionOption.ReplaceExisting);

            try
            {
                using (IRandomAccessStream stream = await companyViewModelFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outputStream = stream.AsStreamForWrite())
                    {
                        serializer.WriteObject(outputStream, this);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }             
        }

        /// <summary>
        /// Load Properties from parameter to current CompanyViewModel
        /// If any of properties from parameter is invalid, don't load them
        /// </summary>
        private void LoadProperties(CompanyViewModel model)
        {
            //in case that loaded data is incorrect do nothing
            if (model.CheckValidation())
            {
                this.Address = model.Address;
                this.CompanyName = model.CompanyName;
                this.Creator = model.Creator;
                this.EmailAddress = model.EmailAddress;
                this.NIP = model.NIP;
                this.PhoneNumber = model.PhoneNumber;
                this.REGON = model.REGON;
                OnPropertyChanged();
            }


        }
        /// <summary>
        /// Load CompanyViewModel from File
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> Load()
        {
            //Data serializer
            DataContractSerializer serializer = new DataContractSerializer(typeof(CompanyViewModel));
            //%appdata%/Local/Packages/*appfolder*/LocalCache/
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile companyViewModelFile = await
                localFolder.CreateFileAsync("companyView.xml", CreationCollisionOption.OpenIfExists);

            //If opening file can't be done, or data is invalid, don't break the app and return false
            try
            {
                using (IRandomAccessStream stream = await companyViewModelFile.OpenAsync(FileAccessMode.Read))
                {
                    using (Stream inputStream = stream.AsStreamForRead())
                    {
                        var result = serializer.ReadObject(inputStream);
                        if (result != null && result is CompanyViewModel viewModel)
                        {
                            this.LoadProperties(viewModel);
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
