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
using ClassLibrary;

namespace ComissionGeneratorUWP.ViewModel
{
    [DataContract]
    abstract public class BaseViewModelUWP : BindableBase
    {
        #region Properties

        protected string ViewModelName { get; private set; }
        protected ViewModelTypeEnumerator ViewModelType { get; private set; }

        #endregion



        #region Constructors

        public BaseViewModelUWP(string viewModelName)
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

        #endregion


        #region Methods

        protected async Task<bool> Load(object sender, ExtensionType extensionType)
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
                        var result = serializer.ReadObject(inputStream);
                        if (result != null)
                        {
                            LoadProperties(result);
                            return true;
                        }
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
        /// Save properties from derived class markes as [DataMember] to file
        /// with extension type given in parameter
        /// </summary>
        /// <returns> true if saving operation succed, false otherwise</returns>
        protected async Task<bool> Save(object sender, ExtensionType extensionType)
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


        /// <summary>
        /// This method needs to be overwritten in derived class
        /// </summary>
        /// <param name="viewModel"></param>
        protected abstract void LoadProperties(object viewModel);

        #endregion


        #region Enumerators

        protected enum ViewModelTypeEnumerator
        {
            CompanyViewModelType,
            ClientViewModelType,
            CommisionViewModelType
        };

        protected enum ExtensionType
        {
            Json, Xml
        };

        #endregion






    }
}
