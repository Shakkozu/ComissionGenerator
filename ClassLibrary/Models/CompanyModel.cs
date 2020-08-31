namespace ClassLibrary.Models
{
    public class CompanyModel
    {
        public CompanyModel()
        {

        }

        public CompanyModel(int id,string nIP, string postalCode, string city, string street, string emailAddress, string companyName, string phoneNumber, string regon)
        {
            Id = id;
            NIP.Number = nIP;
            Address.PostalCode.Number = postalCode;
            Address.City = city;
            Address.Street = street;
            EmailAddress.Address = emailAddress;
            CompanyName = companyName;
            PhoneNumber.Number = phoneNumber;
            REGON.Number = regon;
        }

        public int Id { get; set; }

        public EmailAddressModel EmailAddress { get; set; } = new EmailAddressModel();
        public AddressModel Address { get; set; } = new AddressModel();
        public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();


        public NIPModel NIP { get; set; } = new NIPModel();
        public RegonModel REGON { get; set; } = new RegonModel();

        public string CompanyName { get; set; }

        public override string ToString()
        {
            return $"{CompanyName}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{NIP}\n{REGON}";
        }
    }
}
