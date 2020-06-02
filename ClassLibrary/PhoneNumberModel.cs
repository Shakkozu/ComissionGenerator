using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class PhoneNumberModel
    {
        string Number { get { return _number; } 
            set 
            { 
                if(ValidateNumber(value)==true)
                {
                    _number = value;
                }
                else
                {
                    throw new System.Exception("Invalid phone-number format");
                }
            } 
        }
        string _number;

        /// <summary>
        /// regex validator
        /// valid format - '000-000-000 / 000 000 000'
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool ValidateNumber(string number)
        {
            Regex rx_ = new Regex(@"^(\+\d{2}\s)?\d{3}[\-]\d{3}[\-]\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rx = new Regex(@"^(\+\d{2}\s)?\d{3}[\s]\d{3}[\s]\d{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (rx_.Match(number).Success)
                return true;
            else if (rx.Match(number).Success)
                return true;
            else return false;

        }
    }
}