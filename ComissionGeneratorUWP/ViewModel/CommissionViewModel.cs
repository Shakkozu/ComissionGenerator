using ClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace ComissionGeneratorUWP.ViewModel
{
    [DataContract]
    public class CommissionViewModel : BaseViewModel
    {
        #region Properties

        [DataMember]public ObservableCollection<ItemModel> ItemList = new ObservableCollection<ItemModel>();



        #endregion

        #region Constructors

        public CommissionViewModel() : base("comissionViewModel")
        {
            for (int i = 1; i <= 20; i++)
                ItemList.Add(new ItemModel(i));
        }

        #endregion


        #region Methods

        /// <summary>
        /// Save current properties to file in xml format
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> SaveXml()
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

        internal void GenerateDocument()
        {
           
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
        
        /// <summary>
        /// Todo dokonczyć
        /// </summary>
        /// <param name="viewModel"></param>
        protected override void LoadProperties(object viewModel)
        {
            if(viewModel is CommissionViewModel model)
            {
                if(model.ItemList != null)
                {
                    //ItemList = model.ItemList;
                    for(int i=0; i < model.ItemList.Count; i++)
                    {
                        if(model.ItemList[i].ItemDescription != null &&
                            model.ItemList[i].ItemName != null
                            && model.ItemList[i].ItemPrice!=null)
                        {
                            ItemList[i].ItemDescription = model.ItemList[i].ItemDescription;
                            ItemList[i].ItemName = model.ItemList[i].ItemName;
                            ItemList[i].ItemPrice = model.ItemList[i].ItemPrice;
                        }
                        else
                        {
                            ItemList[i] = new ItemModel(ItemList[i].Id);
                        }
                        

                    }
                    OnCollectionChanged(ItemList);
                }
            }
        }

        #endregion





    }
}