using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace ClassLibrary.DataModels
{
    public class ClientMVCModel
    {


        public int Id { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }


        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Street { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"\d{2}[\s\-]?\d{3}$", ErrorMessage = "Nieprawidłowy kod pocztowy, prawidłowy format:\n ##-###")]
        public string PostalCode { get; set; }

        [Display(Name = "Contact Number")]
        [Required]
        [RegularExpression(@"^(\+\d{2})?\s?\d{3}[\s\-]?\d{3}[\s\-]?\d{3}$", ErrorMessage = "Nieprawidłowy number telefonu, prawidłowy format:\n ###-###-###")]
        public string PhoneNumber { get; set; }

        //        [RegularExpression(@"^\d{3}[\-\s]?\d{3}[\-\s]?\d{2}[\-\s]?\d{2}$", ErrorMessage = "Nieprawidłowy NIP, prawidłowy format:\n ###-###-##-##")]
        [Remote(action: "VerifyIfCompanyNIP", controller: "Clients", AdditionalFields = "Company", ErrorMessage = "Nieprawidłowy NIP, prawidłowy format:\n ###-###-##-##")]
        public string NIP { get; set; } = "";
        //******


        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string LastName { get; set; }

        [Remote(action: "VerifyIfCompanyNameIsValid", controller: "Clients", AdditionalFields = "Company", ErrorMessage ="Prawidłowa nazwa firmy ma przynajmniej 2 znaki. ")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = "";


        [Remote(action: "VerifyIfCompany", controller: "Clients", AdditionalFields = "CompanyName, NIP", ErrorMessage ="Przy zaznaczonym polu \"Company\" Pola Company Name oraz NIP są wymagane! ")]
        public bool Company { get; set; } = false;

        [Display(Name = "Full Name")]
        public string FullName { get
            {
                if(CompanyName != null && CompanyName.Length > 0 && NIP != null && NIPModel.Validate(NIP))
                {
                    return $"\"{ CompanyName }\" { Name } { LastName }";
                }
                else
                {
                    return $"{ Name } { LastName }";
                }
            } }

        
    }
}
