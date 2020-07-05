using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class CommissionCreatorModel
    {

        #region Properties

        public string Name { get; set; }
        public string LastName { get; set; }

        public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();
        public EmailAddressModel EmailAddress { get; set; } = new EmailAddressModel();


        #endregion

        //***********************

        #region Constructors

        public CommissionCreatorModel()
        {
            Name = "";
            LastName = "";
        }

        #endregion

        //***********************

        #region Methods

        public override string ToString()
        {
            return $"{Name} {LastName}\n{EmailAddress}\n{PhoneNumber}";
        }

        #endregion
    }
}
