using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class RegonModel :BindableBase
    {

        #region Properties

        private string _number;

        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                if (Validate(value))
                {
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
                OnPropertyChanged("IsValid");

            }
        }

        public bool IsValid { get; private set; } = true;


        #endregion

        //***********************

        #region Constructors

        public RegonModel()
        {
            _number = "";
        }


        #endregion

        //***********************

        #region Methods

        /// <summary>
        /// Regex Validator,
        /// valid formats: {xxxxxxxxx;xxx xxx xxx; xxx-xxx-xxx}
        /// '-' and spaces can be swapped
        /// </summary>
        /// <returns>true if number is valid REGON format, false otherwise</returns>
        public static bool Validate(string number)
        {
            Regex rx_ = new Regex(@"^\d{3}[\-\s]?\d{3}[\-\s]?\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return (rx_.Match(number).Success);
        }
        public override string ToString()
        {
            return $"REGON: {Number}";
        }

        #endregion


    }
}
