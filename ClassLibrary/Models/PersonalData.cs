namespace ClassLibrary.Models
{
    public class PersonalData
    {
        public CompanyModel Company { get; } = new CompanyModel();
        public CommissionCreatorModel CommissionCreator { get; } = new CommissionCreatorModel();
        public ClientModel Client { get; } = new ClientModel();


        public PersonalData(CompanyViewModel companyViewModel, ClientViewModel clientViewModel)
        {
            Company = companyViewModel.Company;
            CommissionCreator = companyViewModel.Creator;
            Client = clientViewModel.Client;
        }






    }
}
