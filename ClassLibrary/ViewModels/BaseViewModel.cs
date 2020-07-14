using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ClassLibrary
{
        [DataContract]
        abstract public class BaseViewModel : BindableBase
        {
            #region Properties

            protected string ViewModelName { get; private set; }
            protected ViewModelTypeEnumerator ViewModelType { get; private set; }

            #endregion



            #region Constructors

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
                    case "settingsViewModel":
                        ViewModelType = ViewModelTypeEnumerator.SettingsViewModelType;
                        break;
                }

            }

            #endregion


            #region Methods

            protected bool Load(object sender, ExtensionType extensionType)
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
            var directory = Environment.SpecialFolder.ApplicationData;

            

            //IStorageFile clientViewModelFile = await
            //    localFolder.CreateFileAsync(ViewModelName + fileExtenstion, CreationCollisionOption.OpenIfExists);

            try
            {
                var file = File.OpenRead($"{directory}/{ViewModelName}{fileExtenstion}");
                using (StreamReader reader = new StreamReader(file))
                {
                        var result = serializer.ReadObject(reader.BaseStream);
                        if (result != null)
                        {
                            LoadProperties(result);
                            return true;
                        }
                        return false;
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
            protected bool Save(object sender, ExtensionType extensionType)
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

            var directory = Environment.SpecialFolder.ApplicationData;
            if (!Directory.Exists(directory.ToString()))
                Directory.CreateDirectory(directory.ToString());

            

            try
            {
                var file = File.Create($"{directory}/{ViewModelName}{fileExtenstion}");
                using (StreamWriter writer= new StreamWriter(file))
                {

                    serializer.WriteObject(writer.BaseStream, sender);
                        return true;
                    
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
                CommisionViewModelType,
                SettingsViewModelType,
            };

            protected enum ExtensionType
            {
                Json, Xml
            };

            #endregion



        }
    }