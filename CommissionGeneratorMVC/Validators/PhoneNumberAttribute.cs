using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommissionGeneratorMVC.Validators
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public PhoneNumberAttribute(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public string PhoneNumber { get; }

        public string GetMerrorMessage() => 
            $"Valid Phone number formats are: xxx-xxx-xxx, xxx xxx xxx, xxxxxxxxx";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PhoneNumberModel phoneNumberModel = new PhoneNumberModel();
            phoneNumberModel.Number = PhoneNumber;
            if(!phoneNumberModel.IsValid)
            {
                return new ValidationResult(GetMerrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}