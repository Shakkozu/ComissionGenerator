using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Windows.ApplicationModel.Activation;

namespace ClassLibrary.DataModels
{
    public class ItemMVCModel
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal Cost { get; set; }

        [Display(Name = "Item Name")]
        [Required]
        public string ItemName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Item Unit")]
        public ItemUnit ItemUnit { get; set; }

        public decimal Quantity { get; set; }

        public decimal TotalPrice { get { return Quantity * Cost; } } 





    }
}
