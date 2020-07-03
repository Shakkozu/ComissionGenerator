using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ClassLibrary
{
    public class PostalCodeModel : BindableBase
    {
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
        private string _number;
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
