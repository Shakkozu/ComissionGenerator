using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.DataModels
{
    public class CreatorMVCModel
    {
        public int Id { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        [Required]
        [RegularExpression(@"^(\+\d{2})?\s?\d{3}[\s\-]?\d{3}[\s\-]?\d{3}$", ErrorMessage = "Nieprawidłowy numer telefonu, prawidłowy format:\n ###-###-###")]
        public string PhoneNumber { get; set; }


        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [Display(Name="Last Name")]
        [StringLength(60, MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return $"{ Name } { LastName }";
                
            }
        }
    }
}