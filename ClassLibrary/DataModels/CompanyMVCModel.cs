using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassLibrary.DataModels
{
    public class CompanyMVCModel
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

        [Required]
        [RegularExpression(@"^\d{3}[\-\s]?\d{3}[\-\s]?\d{2}[\-\s]?\d{2}$", ErrorMessage = "Nieprawidłowy NIP, prawidłowy format:\n ###-###-##-##")]
        public string NIP { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}[\-\s]?\d{3}[\-\s]?\d{3}$", ErrorMessage = "Nieprawidłowy REGON, prawidłowy format:\n ##########")]
        public string REGON { get; set; }

        [Display(Name = "Company Name")]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CompanyName { get; set; }

    }
}
