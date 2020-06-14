using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Windows.UI.Composition.Scenes;
using Windows.UI.Popups;

namespace ComissionGeneratorUWP
{
    public class CompanyViewModel : BindableBase

    {
        #region Properties
        public AddressModel Address { get; set; } = new AddressModel();
        public PhoneNumberModel PhoneNumber { get; set; } = new PhoneNumberModel();
        
        public CommissionCreator Creator { get; set; } = new CommissionCreator();
        public EmailAddressAttribute EmailAddress = new EmailAddressAttribute();

        public NIPModel NIP { get; set; } = new NIPModel();
        public REGON REGON { get; set; } = new REGON();

        public string CompanyName { get; set; }
        #endregion

        //**************************

        #region Constructors
        public CompanyViewModel()
        {
            
        }
        #endregion


        //**************************

        #region Methods
        /// <summary>
        /// Check if any of properties is incorrect
        /// </summary>
        /// <returns>if all data is OK returns true, false otheriwse</returns>
        public bool CheckValidation()
        {
            if (!Address.PostalCode.IsValid || !PhoneNumber.IsValid ||
                !Creator.PhoneNumber.IsValid || !NIP.IsValid || !REGON.IsValid)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
