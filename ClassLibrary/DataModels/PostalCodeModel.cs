﻿using System.Text.RegularExpressions;


namespace ClassLibrary
{
    public class PostalCodeModel : BindableBase
    {

        #region Private Members

        private string _number;

        #endregion

        //***********************

        #region Properties

        public string Number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
                if (Validate(value) == true)
                {
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
                OnPropertyChanged("IsValid");
                OnPropertyChanged("Number");
            }
        }

        public bool IsValid { get; private set; } = true;


        #endregion

        //***********************

        #region Constructors

        public PostalCodeModel()
        {
            _number = "";
        }

        #endregion

        //***********************

        #region Methods

        /// <summary>
        /// Regex Validator,
        /// valid formats: {xx-xxx; xx xxx; xxxxx}
        /// </summary>
        /// <returns>true if number is valid postal-code format, false otherwise</returns>
        public static bool Validate(string number)
        {
            Regex rx = new Regex(@"^\d{2}[\-\s]?\d{3}$");
            return rx.Match(number).Success;
        }

        public override string ToString()
        {
            return this.Number;
        }

        #endregion



    }
}
