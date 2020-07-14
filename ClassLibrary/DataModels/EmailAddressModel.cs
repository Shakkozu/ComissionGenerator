using System;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class EmailAddressModel : BindableBase
    {
        #region Private Members

        string _address;

        #endregion

        //***********************

        #region Properties

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
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

        public EmailAddressModel(string emailAddress)
        {
            Address = emailAddress;
        }

        public EmailAddressModel()
        {
            _address = "";
        }



        #endregion

        //***********************

        #region Methods

        /// <summary>
        /// Email address regex validator
        /// </summary>
        /// <returns>true if number is valid email address format, false otherwise</returns>
        public static bool Validate(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }
        public override string ToString()
        {
            return $"Email: {Address}";
        }

        #endregion

    }
}
