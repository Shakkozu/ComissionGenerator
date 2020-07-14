namespace ClassLibrary
{
    public class CommissionCreatorModel
    {

        #region Properties

        public string Name { get; set; }
        public string LastName { get; set; }

        virtual public string FullName { get { return $"{Name} {LastName}"; } }

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
            return $"{FullName}\n{EmailAddress}\n{PhoneNumber}";
        }

        #endregion
    }
}
