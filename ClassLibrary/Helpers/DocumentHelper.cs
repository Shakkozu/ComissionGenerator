using ClassLibrary.Models;
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
            newItem.ReplaceText("<ProductPrice>", "$ " + unitPrice.ToString("N2"));
            newItem.ReplaceText("<ProductQuantity>", unitQuantity.ToString());
            newItem.ReplaceText("<TotalPrice>", "$ " + (unitPrice * unitQuantity).ToString("N2"));
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
        /// Function Generates new Document using file template 
        /// </summary>
        /// <param name="template">path to file template</param>
        /// <param name="fileName">path to result file</param>
        /// <param name="UserData">Data about Creator, Client and Company</param>
        /// <param name="Products">List of products</param>
        public static void GenerateDocumentFromTemplate(string template, string fileName, PersonalData UserData, List<ItemModel> Products)
        {
            Id = 1;
            File.Copy(template, fileName, true);
            string tableDescription = "WARES_TABLE";

            DocX doc = DocX.Load(fileName);

            if (doc.FindAll("<CompanyInfo>").Count > 0)
            {
                doc.ReplaceText("<CompanyInfo>", UserData.Company.ToString());
            }
            else
            {
                doc.ReplaceText("<CompanyName>", UserData.Company.CompanyName);
                doc.ReplaceText("<CompanyEmail>", UserData.Company.EmailAddress.ToString());
                doc.ReplaceText("<CompanyAddress>", UserData.Company.Address.ToString());
                doc.ReplaceText("<CompanyPhoneNumber>", UserData.Company.PhoneNumber.ToString());
                doc.ReplaceText("<CompanyNIP>", UserData.Company.NIP.ToString());
                doc.ReplaceText("<CompanyREGON>", UserData.Company.REGON.ToString());
            }

            if (doc.FindAll("<ClientInfo>").Count > 0)
            {
                doc.ReplaceText("<ClientInfo>", UserData.Client.ToString());
            }
            else
            {
                doc.ReplaceText("<ClientName>", UserData.Client.FullName);
                doc.ReplaceText("<ClientEmail>", UserData.Client.EmailAddress.ToString());
                doc.ReplaceText("<ClientAddress>", UserData.Client.Address.ToString());
                doc.ReplaceText("<ClientPhoneNumber>", UserData.Client.PhoneNumber.ToString());
                if(UserData.Client.Company)
                {
                    doc.ReplaceText("<ClientNIP>", UserData.Client.NIP.ToString());
                }
                else
                {
                    doc.ReplaceText("<ClientNIP>","");
                }
            }

            if (doc.FindAll("<CreatorInfo>").Count > 0)
            {
                doc.ReplaceText("<CreatorInfo>", UserData.CommissionCreator.ToString());
            }
            else
            {
                doc.ReplaceText("<CreatorName>", UserData.CommissionCreator.FullName);
                doc.ReplaceText("<CreatorEmail>", UserData.CommissionCreator.EmailAddress.ToString());
                doc.ReplaceText("<CreatorPhoneNumber>", UserData.CommissionCreator.PhoneNumber.ToString());
            }

            var dataTables = doc.Tables.Where(x => x.TableCaption == tableDescription);
            if (dataTables.Count() == 0)
            {
                throw new Exception("Couldn't find table with 'WARES_TABLE' description!");
            }
            else
            {
                Table dataTable = dataTables.FirstOrDefault();


                //Remove empty Rows //TODO REFACTOR
                List<int> rowsToRemove = new List<int>();
                for (int i = 0; i < dataTable.RowCount; i++)
                {
                    if (dataTable.Rows[i].Cells.Any(x => x.Paragraphs.FirstOrDefault().Text.Length < 1))
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



                if (dataTable.RowCount > 0)
                {
                    Row rowPattern = dataTable.Rows[dataTable.RowCount - 1];

                    foreach (var prod in Products.Where(product => product.ItemName != "" && !product.ItemPrice.Equals("0$")))
                    {
                        // Insert a copy of the rowPattern at the last index in the table.
                        var newItem = dataTable.InsertRow(rowPattern, dataTable.RowCount - 1);

                        // Replace the default values of the newly inserted row.
                        newItem.ReplaceText("<ItemName>", prod.ItemName);
                        newItem.ReplaceText("<ItemId>", Id++.ToString());
                        newItem.ReplaceText("<ItemPrice>",prod.ItemPrice);
                        if (int.TryParse(prod.Quantity, out int quantity))
                        {
                            newItem.ReplaceText("<ItemQuantity>", prod.Quantity);
                        }
                        else
                            newItem.ReplaceText("<ItemQuantity>", "1");
                       
                        newItem.ReplaceText("<ItemDescription>",prod.ItemDescription);
                        newItem.ReplaceText("<ItemTotalPrice>",prod.TotalPrice.ToString()+"$");
                    }


                    dataTable.RemoveRow();
                    decimal totalPrice = Products.Sum(x => x.TotalPrice);
                    doc.ReplaceText("<TotalPrice>", totalPrice.ToString("N2") + "$");

                    doc.ReplaceText("<TodayDate>", DateTime.Now.ToLongDateString());



                    doc.Save();
                    Process.Start("WINWORD.EXE", fileName);
                }
            }


            


        }





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
                if (int.TryParse(product.Quantity, out int quantity))
                {
                    if (quantity > 0)
                        r.Cells[4].Paragraphs.First().Append(product.Quantity.ToString());
                    else
                        r.Cells[4].Paragraphs.First().Append("1");
                }
                else
                    r.Cells[4].Paragraphs.First().Append("1");

                r.Cells[5].Paragraphs.First().Append(product.TotalPrice.ToString() + "$");
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
            Paragraph p = doc.InsertParagraph($"Total Cost :{totalPrice}$");
            p.Alignment = Alignment.right;
            p.LineSpacingBefore = 15;
            p.LineSpacingAfter = 30;
            p.FontSize(12);
            return p;
        }


    }
}
