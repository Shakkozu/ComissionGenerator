using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class NIPModel : BindableBase
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

        public NIPModel()
        {
            _number = "";
        }


        #endregion

        //***********************

        #region Methods

        /// <summary>
        /// regex validator
        /// Correct formats: {xxxxxxxxx;xxx-xxx-xx-xx;xxx xxx xx xx}
        /// spaces and '-' can be swapped
        /// </summary>
        /// <param name="number"></param>
        /// <returns>true if number is valid NIP format, false otherwise</returns>
        public static bool Validate(string number)
        {

            Regex rx_ = new Regex(@"^\d{3}[\-\s]?\d{3}[\-\s]?\d{2}[\-\s]?\d{2}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return (rx_.Match(number).Success);
        }


        public override string ToString()
        {
            return $"NIP: {Number}";
        }

        #endregion




    }
}
