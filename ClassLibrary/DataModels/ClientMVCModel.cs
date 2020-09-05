using ExpressiveAnnotations.Attributes;
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


        [RequiredIf("Company == true", ErrorMessage = "Pole NIP jest wymgane, ponieważ pole Company jest zaznaczone")]
        [AssertThat(@"IsRegexMatch(NIP, '^\\d{3}[\\-\\s]?\\d{3}[\\-\\s]?\\d{2}[\\-\\s]?\\d{2}$')", ErrorMessage = "Nieprawidłowy NIP, prawidłowy format:\n ###-###-##-##")]
        public string NIP { get; set; } = "";


        [RequiredIf("Company == false", ErrorMessage = "Pole \"Name\" jest wymagane, gdy pole \"Company\" Nie jest zaznaczone" )]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }


        [RequiredIf("Company == false", ErrorMessage = "Pole \"Last Name\" jest wymagane, gdy pole \"Company\" Nie jest zaznaczone")]
        [StringLength(60, MinimumLength = 3)]
        public string LastName { get; set; }


        [RequiredIf("Company == true", ErrorMessage = "Pole Company Name jest wymgane, ponieważ pole Company jest zaznaczone" )]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = "";


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
