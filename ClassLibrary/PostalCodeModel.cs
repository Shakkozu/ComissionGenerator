using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ClassLibrary
{
    public class PostalCodeModel
    {
        public string Number
        {
            get
            {
                return _number;
            }

            set
            {
                if (Validate(value) == true)
                {
                    _number = value;
                }
                else
                {
                    throw new ArgumentException("Invalid Postal Code Format");
                }
            }
        }
        private string _number;

        public static bool Validate(string number)
        {
            Regex rx = new Regex(@"^\d{2}\-\d{3}$");
            return rx.Match(number).Success;
        }
    }
}
