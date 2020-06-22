using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Animation;

namespace ComissionGeneratorUWP.ViewModel
{
    [DataContract]
    abstract public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected string ViewModelName { get; private set; }
        protected ViewModelTypeEnumerator ViewModelType { get; private set; }

        public BaseViewModel(string viewModelName)
        {
            ViewModelName = viewModelName;
            switch (ViewModelName)
            {
                case "companyViewModel":
                    ViewModelType = ViewModelTypeEnumerator.CompanyViewModelType;
                    break;
                case "clientViewModel":
                    ViewModelType = ViewModelTypeEnumerator.ClientViewModelType;
                    break;
                case "commissionViewModel":
                    ViewModelType = ViewModelTypeEnumerator.CommisionViewModelType;
                    break;
            }

        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        protected void OnPropertyChanged(string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        /// <summary>
        /// Load properties for specified viewModelType
        /// </summary>
        /// <param name="loadedViewModel"></param>
        /// <returns>true if loading is succesful, false otherwise</returns>
        private bool ReadViewModel(object loadedViewModel)
        {
            if (loadedViewModel != null)
            {
                LoadProperties(loadedViewModel);
                return true;
            }
            return false;
        }



        
        protected async virtual internal Task<bool> SaveJson(object sender)
        {
            return await Save(sender, ExtensionType.Json);
        }

        protected async virtual internal Task<bool> SaveXml(object sender)
        {
            return await Save(sender, ExtensionType.Xml);
        }


        private async Task<bool> Load(object sender, ExtensionType extensionType)
        {
            XmlObjectSerializer serializer;
            string fileExtenstion;
            switch (extensionType)
            {
                case ExtensionType.Json:
                    serializer = new DataContractJsonSerializer(sender.GetType());
                    fileExtenstion = ".json";
                    break;
                case ExtensionType.Xml:
                    serializer = new DataContractSerializer(sender.GetType());
                    fileExtenstion = ".xml";
                    break;
                default:
                    return false;
            }


            //%appdata%/Local/Packages/*appfolder*/LocalCache/
            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile clientViewModelFile = await
                localFolder.CreateFileAsync(ViewModelName + fileExtenstion, CreationCollisionOption.OpenIfExists);

            try
            {
                using (IRandomAccessStream stream = await clientViewModelFile.OpenAsync(FileAccessMode.Read))
                {
                    using (Stream inputStream = stream.AsStreamForRead())
                    {
                        var jsonResult = serializer.ReadObject(inputStream);
                        return ReadViewModel(jsonResult);
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
        /// Load clientViewModel from File
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        protected async internal Task<bool> LoadJson(object sender)
        {
            return await Load(sender, ExtensionType.Json);
        }


        /// <summary>
        /// Save properties from derived class markes as [DataMember] to file
        /// with extension type given in parameter
        /// </summary>
        /// <returns> true if saving operation succed, false otherwise</returns>
        private async Task<bool> Save(object sender, ExtensionType extensionType)
        {
            XmlObjectSerializer serializer;
            string fileExtenstion;
            switch (extensionType)
            {
                case ExtensionType.Json:
                    serializer = new DataContractJsonSerializer(sender.GetType());
                    fileExtenstion = ".json";
                    break;
                case ExtensionType.Xml:
                    serializer = new DataContractSerializer(sender.GetType());
                    fileExtenstion = ".xml";
                    break;
                default:
                    return false;
            }

            IStorageFolder localFolder = ApplicationData.Current.LocalCacheFolder;
            IStorageFile clientViewModelFile = await localFolder.CreateFileAsync(ViewModelName + fileExtenstion,
                CreationCollisionOption.ReplaceExisting);

            try
            {
                using (IRandomAccessStream stream = await clientViewModelFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outputStream = stream.AsStreamForWrite())
                    {
                        serializer.WriteObject(outputStream, sender);
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

        
        protected async internal Task<bool> LoadXml(object sender)
        {
            return await Load(sender, ExtensionType.Xml);
        }

        /// <summary>
        /// This method needs to be overwritten in derived class
        /// </summary>
        /// <param name="viewModel"></param>
        protected abstract void LoadProperties(object viewModel);
        
        protected enum ViewModelTypeEnumerator
        {
            CompanyViewModelType,
            ClientViewModelType,
            CommisionViewModelType
        };

        private enum ExtensionType
        {
            Json, Xml
        };
    }
}
