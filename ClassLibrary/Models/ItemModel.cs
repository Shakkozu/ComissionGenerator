using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using Windows.UI.WebUI;

namespace ClassLibrary
{
    [DataContract]
    public class ItemModel : BindableBase
    {

        #region Private Members

        private decimal _itemPrice;
        private string _itemDescription;
        private string _itemName;

        private string _quantityStr;
        private int _quantity;

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
            get { return $"{_itemPrice}$"; }
            set
            {
                if (value.Contains("zł"))
                    value = value.Substring(0, value.Length - 2);
                else if (value.Contains("$"))
                    value = value.Substring(0, value.Length - 1);
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

        public string Quantity
        {
            get
            {
                return _quantityStr;
            }
            set
            {
                if (value.Contains("m"))
                {
                    if (int.TryParse(value.Remove(value.IndexOf("m")), out _quantity))
                    {

                        _quantityStr = _quantity.ToString() + "m";
                    }

                }
                else
                {
                    if (int.TryParse(value, out _quantity))
                    {
                        _quantityStr = _quantity.ToString();
                    }
                }
            }
        }

       

        public decimal TotalPrice { get { return _quantity * _itemPrice; } }

        #endregion


        #region Constructors

        public ItemModel(int id)
        {
            Id = id;
            _itemName = "";
            _itemPrice = 0;
            _itemDescription = "";
            _quantity = 1;
            _quantityStr = "";
        }

        #endregion

        #region Methods



        #endregion









    }
}
