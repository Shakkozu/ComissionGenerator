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
                IsValid = Validate(value, out _number);

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
        public static bool Validate(string number, out string _number)
        {
            Regex rx_ = new Regex(@"^\d{2}[\-]\d{3}$");
            Regex rxS = new Regex(@"^\d{2}[\s]\d{3}$");
            Regex rx = new Regex(@"^\d{5}$");
            _number = number;
            if(rx_.Match(number).Success)
            {
                
                return true;
            }
            else if(rxS.Match(number).Success)
            {
                _number = _number.Replace(" ", "-");
                
            } 
            else if(rx.Match(number).Success)
            {
                _number = _number.Insert(2, "-");
            }
            else
            {
                return false;
            }
            return rx_.Match(_number).Success;
        }
       
        public override string ToString()
        {
            return this.Number;
        }

        #endregion



    }
}
