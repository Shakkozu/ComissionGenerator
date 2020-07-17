using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace ClassLibrary
{
    public class ClientViewModel : BaseViewModel
    {

        #region Properties

        public ClientModel Client { get; set; } = new ClientModel();

        public ObservableCollection<ClientModel> Clients { get; set; } = GetClientsFromDataBase();



        #endregion

        #region Events

        public event EventHandler ClientLoaded;

        #endregion

        #region Constructors

        public ClientViewModel() : base("clientViewModel")
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
            if (!Client.PhoneNumber.IsValid | !Client.Address.PostalCode.IsValid | !Client.EmailAddress.IsValid)
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
                if (model.Client.Address != null)
                {
                    if (model.Client.Address.City != null) Client.Address.City = model.Client.Address.City;
                    if (model.Client.Address.PostalCode.IsValid) Client.Address.PostalCode = model.Client.Address.PostalCode;
                    if (model.Client.Address.Street != null) Client.Address.Street = model.Client.Address.Street;
                    OnPropertyChanged("Address");
                }
                if (model.Client.PhoneNumber.IsValid)
                {
                    Client.PhoneNumber = model.Client.PhoneNumber;
                    OnPropertyChanged("PhoneNumber");
                }
                if (model.Client.Name != null)
                {
                    Client.Name = model.Client.Name;
                    OnPropertyChanged("Name");
                }

                if (model.Client.LastName != null)
                {
                    Client.LastName = model.Client.LastName;
                    OnPropertyChanged("LastName");
                }
                if (model.Client.EmailAddress != null)
                {
                    Client.EmailAddress = model.Client.EmailAddress;
                    OnPropertyChanged("EmailAddress");
                }

            }

        }


        /// <summary>
        /// Remove client given in parameter from database and reload it.
        /// </summary>
        /// <param name="selectedClient"></param>
        public void RemoveClient(ClientModel selectedClient)
        {
            SQLiteDataAccess.RemoveClient(selectedClient);
            Clients = GetClientsFromDataBase();
            OnPropertyChanged("Clients");
        }

        //Load clients from database
        private static ObservableCollection<ClientModel> GetClientsFromDataBase()
        {
            ObservableCollection<ClientModel> result = new ObservableCollection<ClientModel>(SQLiteDataAccess.LoadClients());
            return result;
        }

        /// <summary>
        /// Save client into Clients database 
        /// </summary>
        public void AddClient()
        {   
            SQLiteDataAccess.SaveClient(Client);
            Clients = GetClientsFromDataBase();
            OnPropertyChanged("Clients");
        }

        /// <summary>
        /// Load selected client
        /// </summary>
        /// <param name="selectedClient"></param>
        public void LoadClient(ClientModel selectedClient)
        {
            Client = selectedClient;
            OnPropertyChanged("Client");
            ClientLoaded?.Invoke(this, new EventArgs());
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
