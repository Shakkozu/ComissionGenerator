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
        public static bool Validate(string number)
        {
            Regex rx_ = new Regex(@"^(\+\d{2})?\s?\d{3}[\s\-]?\d{3}[\-]?\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rx = new Regex(@"^(\+\d{2})?\s?\d{3}[\s]?\d{3}[\s]?\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (rx_.Match(number).Success)
                return true;
            else if (rx.Match(number).Success)
                return true;
            else return false;

        }
        public override string ToString()
        {
            return $"Phone Number: {Number}";
        }

        #endregion




    }

}