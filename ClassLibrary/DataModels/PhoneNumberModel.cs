using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class PhoneNumberModel : BindableBase
    {

        #region Private Members
        
        string _number;

        #endregion

        //***********************

        #region Properties

        public string Number
        {
            get { return _number; }
            set
            {
                IsValid = Validate(value, out _number);
               
                OnPropertyChanged("IsValid");
                OnPropertyChanged("Number");

            }
        }
        public bool IsValid { get; private set; } = true;
       

        #endregion

        //***********************

        #region Constructors

        public PhoneNumberModel(string number)
        {
            Number = number;
        }

        public PhoneNumberModel()
        {
            _number = "";
        }

        #endregion

        //***********************

        #region Methods

        /// <summary>
        /// regex validator
        /// valid format - '000-000-000 / 000 000 000'
        /// </summary>
        /// <returns>true if number is valid phone-number format, false otherwise</returns>
        public static bool Validate(string number, out string _number)
        {
            Regex rx_ = new Regex(@"^(\+\d{2}\s)?\d{3}[\-]\d{3}[\-]\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rxS = new Regex(@"^(\+\d{2}\s)?\d{3}[\s]\d{3}[\s]\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rx = new Regex(@"^(\+\d{2}\s)?\d{3}\d{3}\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
           
            _number = number;

            if (rx_.Match(number).Success)
            {
                return true;
            }
            else if (rx.Match(number).Success)
            {
                if (number.Length > 9)
                {
                    _number = _number.Insert(7, "-");
                    _number = _number.Insert(11, "-");
                }
                else
                {
                    _number = _number.Insert(3, "-");
                    _number = _number.Insert(7, "-");
                }
            }
            else if (rxS.Match(number).Success)
            {
                if (number.Length > 12)
                {
                    string dialCode = _number.Substring(0,4);
                    string n = _number.Substring(4);
                    n = n.Replace(" ", "-");
                    _number = dialCode + n;
                }
                else
                    _number = _number.Replace(" ", "-");
            }
            else
            {
                return false;
            }
            return rx_.Match(_number).Success;
        }
        public override string ToString()
        {
            return $"Phone Number: {Number}";
        }

        #endregion




    }
        
}