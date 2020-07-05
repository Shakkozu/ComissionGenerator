using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace ClassLibrary
{
    [DataContract]
    public class ItemModel : BindableBase
    {

        #region Private Members

        private decimal _itemPrice;
        private string _itemDescription;
        private string _itemName;

        #endregion

        #region Properties

        [DataMember] public string ItemName { get { return _itemName; }
            set 
            {
                _itemName = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string ItemPrice
        {
            get { return $"{_itemPrice}zł"; }
            set
            {
                if (value.Contains("zł"))
                    value = value.Substring(0, value.Length - 2);
                if (decimal.TryParse(value.Replace('.', ','), out decimal result))
                    _itemPrice = result;

                OnPropertyChanged();
            }
        }
        
        [DataMember] public string ItemDescription { get { return _itemDescription; } 
            set 
            { 
                _itemDescription = value; 
                OnPropertyChanged(); 
            } 
        }
        [DataMember] public int Id { get; private set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get { return Quantity * _itemPrice; } }

        #endregion

        #region Constructors

        public ItemModel(int id)
        {
            Id = id;
            _itemName = "";
            _itemPrice = 0;
            _itemDescription = "";
        }

        #endregion

        #region Methods



        #endregion









    }
}
