using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ComissionGeneratorUWP.ViewModel
{
    [KnownType(typeof(ClientViewModel))]
    public class ClientViewModel :  INotifyPropertyChanged
    {

        #region Properties

        [DataMember] public string Name { get; set; }
        [DataMember] public string LastName { get; set; }

        [DataMember] public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();
        [DataMember] public AddressModel Address { get; set; } = new AddressModel();

        #endregion

        public ClientViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            if (!PhoneNumber.IsValid | !Address.PostalCode.IsValid)
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
            DataContractSerializer serializer = new DataContractSerializer(typeof(ClientViewModel));
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile clientViewModelFile = await localFolder.CreateFileAsync("clientView.xml", CreationCollisionOption.ReplaceExisting);

            try
            {
                using (IRandomAccessStream stream = await clientViewModelFile.OpenAsync(FileAccessMode.ReadWrite))
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
        /// Load Properties from parameter to current clientViewModel
        /// Function loads only valid data
        /// PS.Unfortunately overloading operator '=' is not possible so data overwriting needs to be written this way
        /// </summary>
        private void LoadProperties(ClientViewModel model)
        {
            if (model.Address != null) Address = model.Address;
            if (model.PhoneNumber.IsValid) PhoneNumber = model.PhoneNumber;
            if (model.Name != null) Name = model.Name;
            if (model.LastName != null) LastName = model.LastName;
            OnPropertyChanged();
        }






        /// <summary>
        /// Load clientViewModel from File
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> Load()
        {
            //Data serializer
            DataContractSerializer serializer = new DataContractSerializer(typeof(ClientViewModel));


            //%appdata%/Local/Packages/*appfolder*/LocalCache/
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile clientViewModelFile = await
                localFolder.CreateFileAsync("clientView.xml", CreationCollisionOption.OpenIfExists);

            //If opening file can't be done, or data is invalid, don't break the app and return false
            try
            {
                using (IRandomAccessStream stream = await clientViewModelFile.OpenAsync(FileAccessMode.Read))
                {
                    using (Stream inputStream = stream.AsStreamForRead())
                    {

                        var result = serializer.ReadObject(inputStream);
                        if (result != null && result is ClientViewModel viewModel)
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

        /// <summary>
        /// Load clientViewModel from File
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        async internal Task<bool> LoadJson()
        {
            //Data serializer
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ClientViewModel));
            //%appdata%/Local/Packages/*appfolder*/LocalCache/
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile clientViewModelFile = await
                localFolder.CreateFileAsync("clientView.json", CreationCollisionOption.OpenIfExists);

            try
            {
                using (IRandomAccessStream stream = await clientViewModelFile.OpenAsync(FileAccessMode.Read))
                {
                    using (Stream inputStream = stream.AsStreamForRead())
                    {

                        var jsonResult = jsonSerializer.ReadObject(inputStream);
                        if (jsonResult != null && jsonResult is ClientViewModel viewModel)
                        {
                            this.LoadProperties(viewModel);
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Save current properties to file
        /// </summary>
        /// <returns></returns>
        async internal Task<bool> SaveJson()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientViewModel));
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile clientViewModelFile = await localFolder.CreateFileAsync("clientView.json", CreationCollisionOption.ReplaceExisting);

            try
            {
                using (IRandomAccessStream stream = await clientViewModelFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outputStream = stream.AsStreamForWrite())
                    {
                        serializer.WriteObject(outputStream, this);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }



        #endregion
    }
}
