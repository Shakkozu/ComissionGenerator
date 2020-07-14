﻿using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ClassLibrary.Helpers
{
    public static class DocumentHelper
    {
        #region Properties 

        private static int Id = 1;

        #endregion

        public static void GenerateDocument()
        {
            string resultFileName = @"C:\Users\user\Desktop\test.docx";
            string fileName = @"C:\Users\user\Desktop\openSourceTemplate.docx";
            File.Copy(fileName, resultFileName, true);



            using (DocX doc = DocX.Load(resultFileName))
            {
                var productsTable = doc.Tables.FirstOrDefault(x => x.TableCaption == "PRODUCT_LIST");
                if (productsTable == null)
                {
                    throw new Exception("Cooldn't find table with caption PRODUCT_LIST");
                }
                else
                {

                    if (productsTable.RowCount > 1)
                    {
                        //Get the row pattern of the second row
                        var rowPattern = productsTable.Rows[1];
                        AddItemToTable(productsTable, rowPattern, "Chleb");
                        AddItemToTable(productsTable, rowPattern, "Banan");
                        AddItemToTable(productsTable, rowPattern, "Jabłko");
                        AddItemToTable(productsTable, rowPattern, "Cukier");

                        rowPattern.Remove();
                    }
                    doc.Save();
                }




                Process.Start("WINWORD.EXE", resultFileName);




            }

        }

        private static void AddItemToTable(Table table, Row rowPattern, string productName)
        {
            Random rand = new Random(32432);
            // Gets a random unit price and quantity.
            var unitPrice = Math.Round(rand.NextDouble(), 2);
            var unitQuantity = rand.Next(1, 10);

            // Insert a copy of the rowPattern at the last index in the table.
            var newItem = table.InsertRow(rowPattern, table.RowCount - 1);

            // Replace the default values of the newly inserted row.
            newItem.ReplaceText("<ProductName>", productName);
            newItem.ReplaceText("<ProductId>", Id++.ToString());
            newItem.ReplaceText("<ProductPrice>", "PLN " + unitPrice.ToString("N2"));
            newItem.ReplaceText("<ProductQuantity>", unitQuantity.ToString());
            newItem.ReplaceText("<TotalPrice>", "PLN " + (unitPrice * unitQuantity).ToString("N2"));
        }

        /// <summary>
        /// Function Generates new Document with data passed in arguments
        /// </summary>
        /// <param name="fileName">path to output file</param>
        /// <param name="UserData">Data about Creator, Client and Company</param>
        /// <param name="Products">List of products</param>
        public static void GenerateNewDocument(string fileName, PersonalData UserData, List<ItemModel> Products)
        {
            Id = 1;
            DocX doc = DocX.Create(fileName, DocumentTypes.Document);


            Paragraph p = InsertActualDate(doc);


            /* Personal Data Table */
            InsertPersonalDataTable(doc, out Table personalDataTable, out Row row);
            PrettyPersonalDataTable(row);

            //Fill the second row of Personal Data table with actual Company + Client Data
            row = personalDataTable.Rows[1];
            row.Cells[0].Paragraphs.First().Append(UserData.Company.ToString());

            row.Cells[1].Paragraphs.First().Append(UserData.Client.ToString());


            /* Wares Table */
            var validProducts = from product in Products
                      where product.ItemName != "" && product.ItemPrice != "0$"
                      select product;

            //if There are product filled properly
            //Add table containing products and their price
            if (validProducts.Count() > 0)
            {
                p = InsertProductInfoParagraph(doc);
                InsertWaresTable(doc, out Table waresTable, out Row r);

                r = InsertWares(Products, waresTable, r);
                PrettyWaresTable(waresTable, TableDesign.ColorfulList);
                decimal totalPrice = CalculateWaresTotalPrice(Products);

                if (totalPrice > 0)
                {
                    p = InsertTotalPriceParagraph(doc, totalPrice);
                }

                InsertCommissionGeneratorTable(UserData, doc);

                doc.Save();

                Process.Start("WINWORD.EXE", fileName);
            }
           
        }

        /// <summary>
        /// Function Generates new Document using file template. Table containing products MUST HAVE Caption == "WARES_TABLE"!!
        /// </summary>
        /// <param name="template">path to file template</param>
        /// <param name="fileName">path to result file</param>
        /// <param name="UserData">Data about Creator, Client and Company</param>
        /// <param name="Products">List of products</param>
        public static void GenerateDocumentFromTemplate(string template, string fileName, PersonalData UserData, List<ItemModel> Products, bool replaceOnlyValues)
        {
            Id = 1;
            File.Copy(template, fileName, true);
            string tableCaption = "WARES_TABLE";

            DocX doc = DocX.Load(fileName);

            //Replace Personal Info
            ReplaceCompanyInfo(UserData, doc,replaceOnlyValues);

            ReplaceClientInfo(UserData, doc, replaceOnlyValues);

            ReplaceCreatorInfo(UserData, doc, replaceOnlyValues);

            var dataTables = doc.Tables.Where(x => x.TableCaption == tableCaption);
            if (dataTables.Count() == 0)
            {
                throw new ArgumentException("Couldn't find table with 'WARES_TABLE' description!");
            }
            else
            {
                Table dataTable = dataTables.FirstOrDefault();

                RemoveEmptyTableRows(dataTable);


                //Find rows containing Tags
                if (dataTable.RowCount > 0)
                {
                    int numerator = FindRowsContainingTags(dataTable);

                    foreach (var prod in Products.Where(product => product.ItemName != "" && (!product.ItemPrice.Equals("0"))))
                    {
                        // Insert a copies of the rowPatterns containing tags at end of the table.
                        InsertRowsContainingTemplate(dataTable, numerator);
                       

                        // Replace the default values of the newly inserted rows
                        ReplaceRowInfo(dataTable, "<ItemName>", prod.ItemName);
                        ReplaceRowInfo(dataTable, "<ItemId>", Id++.ToString());

                        
                        ReplaceRowInfo(dataTable, "<ItemQuantity>", prod.Quantity);
                        ReplaceRowInfo(dataTable, "<ItemDescription>", prod.ItemDescription);
                        if(replaceOnlyValues==true)
                        {
                            ReplaceRowInfo(dataTable, "<ItemTotalPrice>", prod.TotalPrice.ToString());
                            ReplaceRowInfo(dataTable, "<ItemPrice>", prod.ItemPrice + "PLN");
                        }
                        else
                        {
                            ReplaceRowInfo(dataTable, "<ItemPrice>", prod.ItemPrice);
                            ReplaceRowInfo(dataTable, "<ItemTotalPrice>", prod.TotalPrice.ToString() + "PLN");
                        }
                       


                    }

                    //Remove rows containing tags
                    for (int i = 0; i < numerator; i++)
                    {
                        dataTable.RemoveRow();
                    }
                    
                    decimal totalPrice = Products.Sum(x => x.TotalPrice);

                    //Replace TotalPrice Info
                    
                        doc.ReplaceText("<TotalPrice>", totalPrice.ToString("N2") + " PLN");

                    //Replace TodayDate Info
                    doc.ReplaceText("<TodayDate>", DateTime.Now.ToLongDateString());



                    doc.Save();
                    Process.Start("WINWORD.EXE", fileName);
                }
            }





        }


        #region Generate Document From Template

        #region Personal Data Table Methods 

        private static void ReplaceCreatorInfo(PersonalData UserData, DocX doc, bool replaceOnlyValues)
        {
            if (doc.FindAll("<CreatorInfo>").Count > 0)
            {
                doc.ReplaceText("<CreatorInfo>", UserData.CommissionCreator.ToString());
            }
            else if(replaceOnlyValues==false)
            {
                doc.ReplaceText("<CreatorName>", UserData.CommissionCreator.FullName);
                doc.ReplaceText("<CreatorEmail>", UserData.CommissionCreator.EmailAddress.ToString());
                doc.ReplaceText("<CreatorPhoneNumber>", UserData.CommissionCreator.PhoneNumber.ToString());
            }
            else
            {
                doc.ReplaceText("<CreatorName>", UserData.CommissionCreator.FullName);
                doc.ReplaceText("<CreatorEmail>", UserData.CommissionCreator.EmailAddress.Address);
                doc.ReplaceText("<CreatorPhoneNumber>", UserData.CommissionCreator.PhoneNumber.Number);
            }
        }

        private static void ReplaceClientInfo(PersonalData UserData, DocX doc, bool replaceOnlyValues)
        {
            if (doc.FindAll("<ClientInfo>").Count > 0)
            {
                doc.ReplaceText("<ClientInfo>", UserData.Client.ToString());
            }
            else if(replaceOnlyValues==false)
            {
                doc.ReplaceText("<ClientName>", UserData.Client.FullName);
                doc.ReplaceText("<ClientEmail>", UserData.Client.EmailAddress.ToString());
                doc.ReplaceText("<ClientAddress>", UserData.Client.Address.ToString());
                doc.ReplaceText("<ClientPhoneNumber>", UserData.Client.PhoneNumber.ToString());
                if (UserData.Client.Company)
                {
                    doc.ReplaceText("<ClientNIP>", UserData.Client.NIP.ToString());
                }
                else
                {
                    doc.ReplaceText("<ClientNIP>", "");
                }
            }
            else
            {
                doc.ReplaceText("<ClientName>", UserData.Client.FullName);
                doc.ReplaceText("<ClientEmail>", UserData.Client.EmailAddress.Address);
                doc.ReplaceText("<ClientAddress>", UserData.Client.Address.ToString()); 
                doc.ReplaceText("<CompanyAddressPostalCode>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyAddressStreet>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyAddressCity>", UserData.Company.Address.ToString());
                doc.ReplaceText("<ClientPhoneNumber>", UserData.Client.PhoneNumber.Number);
                if (UserData.Client.Company)
                {
                    doc.ReplaceText("<ClientNIP>", UserData.Client.NIP.Number);
                }
                else
                {
                    doc.ReplaceText("<ClientNIP>", "");
                }
            }
        }

        private static void ReplaceCompanyInfo(PersonalData UserData, DocX doc, bool replaceOnlyValues)
        {
            if (doc.FindAll("<CompanyInfo>").Count > 0)
            {
                doc.ReplaceText("<CompanyInfo>", UserData.Company.ToString());
            }
            else if(replaceOnlyValues==false)
            {
                doc.ReplaceText("<CompanyName>", UserData.Company.CompanyName);
                doc.ReplaceText("<CompanyEmail>", UserData.Company.EmailAddress.ToString());
                doc.ReplaceText("<CompanyAddress>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyPhoneNumber>", UserData.Company.PhoneNumber.ToString());
                doc.ReplaceText("<CompanyNIP>", UserData.Company.NIP.ToString());
                doc.ReplaceText("<CompanyREGON>", UserData.Company.REGON.ToString());
            }
            else
            {
                doc.ReplaceText("<CompanyName>", UserData.Company.CompanyName);
                doc.ReplaceText("<CompanyEmail>", UserData.Company.EmailAddress.Address);
                doc.ReplaceText("<CompanyAddressPostalCode>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyAddressStreet>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyAddressCity>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyAddress>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyPhoneNumber>", UserData.Company.PhoneNumber.Number);
                doc.ReplaceText("<CompanyNIP>", UserData.Company.NIP.Number);
                doc.ReplaceText("<CompanyREGON>", UserData.Company.REGON.Number);
            }
        }


        #endregion


        #region Wares Table Methods

        private static Row ReplaceRowInfo(Table dataTable, string pattern, string replacingValue)
        {
            var row = dataTable.Rows.FirstOrDefault(x => x.FindUniqueByPattern(pattern,
                                        System.Text.RegularExpressions.RegexOptions.Compiled).Count > 0);
            if (row != null)
            {
                row.ReplaceText(pattern, replacingValue);
            }
            return row;
        }

        private static void InsertRowsContainingTemplate(Table dataTable, int numerator)
        {
            for (int i = 0; i < numerator; i++)
            {
                Row rowPattern = dataTable.Rows[dataTable.RowCount - numerator];
                dataTable.InsertRow(rowPattern);
            }
        }

        private static int FindRowsContainingTags(Table dataTable)
        {
            int numerator = 0;
            foreach (Row row in dataTable.Rows)
            {
                var result = row.FindUniqueByPattern(@"<\w*>", System.Text.RegularExpressions.RegexOptions.Compiled);
                if (result.Count > 0)
                    numerator++;

            }

            return numerator;
        }

        private static void RemoveEmptyTableRows(Table dataTable)
        {
            List<int> rowsToRemove = new List<int>();
            for (int i = 0; i < dataTable.RowCount; i++)
            {
                bool result = dataTable.Rows[i].Cells.Any(x => x.Paragraphs.Any(y => y.Text.Length > 0));
                if (result == false)
                {
                    rowsToRemove.Add(i);
                }



            }

            for (int i = 0; i < rowsToRemove.Count; i++)
            {
                dataTable.RemoveRow(rowsToRemove[i]);
                for (int j = 0; j < rowsToRemove.Count; j++)
                    rowsToRemove[j]--;
            }
        }


        #endregion

        #endregion




        #region Generate New Document

        #region Personal Data Table Methods

        private static void InsertPersonalDataTable(DocX doc, out Table table, out Row row)
        {
            table = doc.InsertTable(2, 2);
            table.AutoFit = AutoFit.Window;
            table.TableCaption = "PERSONAL_DATA_TABLE";
            //Fill the first row of table
            row = table.Rows.First();
            row.Height = 24;
            row.Cells[0].Paragraphs.First().Append("Company Data");
            row.Cells[1].Paragraphs.First().Append("Client Data");
        }

        private static void PrettyPersonalDataTable(Row row)
        {
            foreach (var cell in row.Cells)
            {
                cell.Paragraphs.First().Alignment = Alignment.center;
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.Paragraphs.First().FontSize(14);
            }
        }

        #endregion


        #region Wares Table Methods

        private static Paragraph InsertProductInfoParagraph(DocX doc)
        {
            Paragraph p = doc.InsertParagraph("Products Info");
            p.FontSize(22);
            p.Bold(true);
            p.Alignment = Alignment.center;
            p.SpacingBefore(30);
            p.SpacingAfter(10);
            return p;
        }


        private static void InsertWaresTable(DocX doc, out Table waresTable, out Row r)
        {
            waresTable = doc.InsertTable(1, 6);
            waresTable.TableCaption = "WARES_TABLE";

            r = waresTable.Rows.First();

            FillWaresTableInformationRow(waresTable, r);
        }

        private static void FillWaresTableInformationRow(Table waresTable, Row r)
        {
            waresTable.AutoFit = AutoFit.Contents;
            r.Cells[0].Paragraphs.First().Append("Id");
            r.Cells[1].Paragraphs.First().Append("Product Name");
            r.Cells[2].Paragraphs.First().Append("Product Price");
            r.Cells[3].Paragraphs.First().Append("Product Description");
            r.Cells[4].Paragraphs.First().Append("Quantity");
            r.Cells[5].Paragraphs.First().Append("Price Total");
            r.Cells.ForEach(x => x.Paragraphs.First().Bold(true));
        }

        private static Row InsertWares(List<ItemModel> Products, Table waresTable, Row r)
        {
            foreach (var product in Products.Where(product => product.ItemName != "" && !product.ItemPrice.Equals("0$")))
            {
                r = waresTable.InsertRow();
                r.Cells[0].Paragraphs.First().Append(Id++.ToString());
                r.Cells[1].Paragraphs.First().Append(product.ItemName);
                r.Cells[2].Paragraphs.First().Append(product.ItemPrice);
                r.Cells[3].Paragraphs.First().Append(product.ItemDescription);
                r.Cells[4].Paragraphs.First().Append(product.Quantity);
                r.Cells[5].Paragraphs.First().Append(product.TotalPrice.ToString() + "PLN");
            }

            return r;
        }

        private static void PrettyWaresTable(Table waresTable, TableDesign design)
        {
            waresTable.Rows.ForEach(x => x.Cells.ForEach(y => y.Paragraphs.First().Alignment = Alignment.center));
            waresTable.SetColumnWidth(0, 30);
            waresTable.SetColumnWidth(3, 70);
            waresTable.Design = design;
            waresTable.Alignment = Alignment.center;
        }

        private static decimal CalculateWaresTotalPrice(List<ItemModel> Products)
        {
            decimal totalPrice = 0;
            Products.ForEach(product => totalPrice += product.TotalPrice);
            return totalPrice;
        }


        #endregion


        #region Commission Generator Table

        private static void InsertCommissionGeneratorTable(PersonalData UserData, DocX doc)
        {
            Table commissionGeneratorTable = doc.InsertTable(3, 2);
            commissionGeneratorTable.TableCaption = "COMMISSION_GENERATOR_TABLE";

            commissionGeneratorTable.Rows[0].Cells[0].Paragraphs.FirstOrDefault().Append("Document Generated By");
            commissionGeneratorTable.Rows[0].Cells[1].Paragraphs.FirstOrDefault().Append($"{UserData.CommissionCreator.Name} {UserData.CommissionCreator.LastName} ");

            commissionGeneratorTable.Rows[1].Cells[0].Paragraphs.FirstOrDefault().Append("Email");
            commissionGeneratorTable.Rows[1].Cells[1].Paragraphs.FirstOrDefault().Append($"{UserData.CommissionCreator.EmailAddress.Address}");

            commissionGeneratorTable.Rows[2].Cells[0].Paragraphs.FirstOrDefault().Append("Phone Number");
            commissionGeneratorTable.Rows[2].Cells[1].Paragraphs.FirstOrDefault().Append($"{UserData.CommissionCreator.PhoneNumber.Number}");

            commissionGeneratorTable.Alignment = Alignment.left;
            commissionGeneratorTable.AutoFit = AutoFit.Contents;
        }

        #endregion

        private static Paragraph InsertActualDate(DocX doc)
        {
            var p = doc.InsertParagraph("Generation Date:  " + DateTime.Now.ToShortDateString());
            p.Alignment = Alignment.right;

            p.SpacingLine(10);

            p = doc.InsertParagraph("Commission");
            p.Alignment = Alignment.center;
            p.FontSize(32);
            p.Bold(true);
            p.SpacingLine(15);
            return p;
        }

        private static Paragraph InsertTotalPriceParagraph(DocX doc, decimal totalPrice)
        {
            Paragraph p = doc.InsertParagraph($"Total Cost :{totalPrice}PLN");
            p.Alignment = Alignment.right;
            p.LineSpacingBefore = 15;
            p.LineSpacingAfter = 30;
            p.FontSize(12);
            return p;
        }

        #endregion
    }
}
