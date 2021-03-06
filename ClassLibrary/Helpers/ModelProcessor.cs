﻿using ClassLibrary.DataModels;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ClassLibrary.Helpers
{
    public static class ModelProcessor
    {

        public static CompanyModel ConvertToCompanyModel(this CompanyMVCModel model)
        {
            if (model == null) return new CompanyModel();
            return new CompanyModel(model.Id, model.NIP, model.PostalCode, model.City, model.Street, model.EmailAddress, model.CompanyName, model.PhoneNumber, model.REGON);
        }

        public static ClientModel ConvertToClientModel(this ClientMVCModel model)
        {
            if (model == null) return new ClientModel();
            return new ClientModel(model.Id, model.NIP, model.PostalCode, model.City, model.Street, model.EmailAddress, model.CompanyName, model.Name, model.LastName, model.PhoneNumber);
        }
        public static CommissionCreatorModel ConvertToCommissionCreatorModel(this CreatorMVCModel model)
        {
            if (model == null) return new CommissionCreatorModel();
            return new CommissionCreatorModel(model.Name, model.LastName, model.PhoneNumber, model.EmailAddress);
        }

        public static List<ItemModel> ConvertToItemModel(this List<ItemMVCModel> products)
        {
            List<ItemModel> result = new List<ItemModel>();
            foreach (ItemMVCModel prod in products)
            {
                result.Add(new ItemModel(prod.ItemName, prod.Description, prod.Quantity, prod.ItemUnit, prod.Cost));
            }

            return result;
        }
    }
}
