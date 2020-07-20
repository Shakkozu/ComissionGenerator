using System.Runtime.Serialization;

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

        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                OnPropertyChanged();
            }
        }


        public string ItemPrice
        {
            get { return $"{_itemPrice} $"; }
            set
            {
                if (value.Contains("PLN"))
                {
                    value = value.Substring(0, value.Length - 3);
                }
                else if (value.Contains("$"))
                {
                    value = value.Substring(0, value.Length - 1);
                }
                if (decimal.TryParse(value.Replace('.', ','), out decimal result))
                    _itemPrice = result;

                OnPropertyChanged();
            }
        }

        public string ItemDescription
        {
            get { return _itemDescription; }
            set
            {
                _itemDescription = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; private set; }

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
                        if (_quantity < 1) _quantity = 1;
                        _quantityStr = _quantity.ToString() + "m";
                    }

                }
                else
                {
                    if (int.TryParse(value, out _quantity))
                    {
                        if (_quantity < 1) _quantity = 1;
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
            _quantityStr = "1";
        }

        #endregion

        #region Methods



        #endregion









    }
}
