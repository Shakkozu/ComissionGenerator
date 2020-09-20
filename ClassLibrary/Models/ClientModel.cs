namespace ClassLibrary
{
    public class ClientModel : CommissionCreatorModel
    {
        #region Properties

        public int Id { get; set; }
        public AddressModel Address { get; set; } = new AddressModel();

        public bool Company { get; set; } = false;

        public string CompanyName { get; set; } = "";
        public NIPModel NIP { get; set; } = new NIPModel();

        public override string FullName => Company ? CompanyName : $"{Name} {LastName}";

        //I Assume that valid client has Addres, FirstName + LastName or CompanyName, 
        // and valid email Address or valid Phone number
        public bool IsValid
        {
            get
            {
                if (Address.IsValid && 
                    ((Company==false && Name != "" && LastName != "") || (Company==true && CompanyName != "" && (NIP.IsValid && NIP.Number != ""))) &&
                    ((PhoneNumber.Number != "" && PhoneNumber.IsValid ) ||
                    (EmailAddress.Address != "" && EmailAddress.IsValid)))
                {
                    return true;
                }
                else 
                    return false;
            }
        }


        #endregion

        //***********************

        #region Constructors

        public ClientModel()
        {

        }

        public ClientModel(int id,string nip, string postalCode, string city, string street,string emailAddress, string companyName, string name, string lastName, string phoneNumber)
        {
            Id = id;
            PhoneNumber.Number = phoneNumber;
            if (nip != null)
            {
                NIP.Number = nip;
            }
            Address.PostalCode.Number = postalCode;
            Address.City = city;
            Address.Street = street;
            EmailAddress.Address = emailAddress;
            CompanyName = companyName;
            Name = name;
            LastName = lastName;
            if (companyName != "")
                Company = true;
            else
                Company = false;
        }

        #endregion

        //***********************

        #region Methods

        public override string ToString()
        {
            string footer = Company ? $"{NIP}" : "";
            return $"{FullName}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{footer}";
        }


        #endregion





    }
}
