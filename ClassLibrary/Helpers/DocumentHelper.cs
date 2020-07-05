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
    public class DocumentHelper
    {
        public static int Id { get; private set; } = 0;

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

        ///// <summary>
        ///// Function Generates new Document with data passed in arguments
        ///// </summary>
        ///// <param name="fileName">path to output file</param>
        ///// <param name="UserData">Data about Creator, Client and Company</param>
        ///// <param name="Products">List of products</param>
        //private static void GenerateNewDocument(string fileName, PersonalData UserData, List<ItemModel> Products)
        //{
        //    DocX doc = DocX.Create(fileName, DocumentTypes.Document);

        //    /*
        //     Insert Actual Date
        //     */

        //    var p = doc.InsertParagraph("Generation Date:  " + DateTime.Now.ToShortDateString());
        //    p.Alignment = Alignment.right;

        //    p.SpacingLine(10);

        //    p = doc.InsertParagraph("Commission");
        //    p.Alignment = Alignment.center;
        //    p.FontSize(32);
        //    p.Bold(true);
        //    p.SpacingLine(15);



        //    /*
        //     Insert Table with Creator and Client info
        //     */

        //    var table = doc.InsertTable(2, 2);
        //    table.AutoFit = AutoFit.Window;

        //    //Fill the first row of table
        //    var row = table.Rows.First();
        //    row.Height = 24;
        //    row.Cells[0].Paragraphs.First().Append("Company Data");
        //    row.Cells[1].Paragraphs.First().Append("Client Data");

        //    //foreach col in cow
        //    //Change it to LINQ in future
        //    foreach (var cell in row.Cells)
        //    {
        //        cell.Paragraphs.First().Alignment = Alignment.center;
        //        cell.VerticalAlignment = VerticalAlignment.Center;
        //        cell.Paragraphs.First().FontSize(14);
        //    }

        //    //Fill the second row with actual Company + Client Data
        //    row = table.Rows[1];
        //    row.Cells[0].Paragraphs.First().Append(UserData.CompanyData);

        //    row.Cells[1].Paragraphs.First().Append(UserData.ClientData);

        //    p = doc.InsertParagraph("Products Info");
        //    p.FontSize(22);
        //    p.Bold(true);
        //    p.Alignment = Alignment.center;
        //    p.SpacingBefore(30);
        //    p.SpacingAfter(10);


        //    Table waresTable = doc.InsertTable(1, 5);
        //    var r = waresTable.Rows.First();

        //    waresTable.AutoFit = AutoFit.Contents;
        //    r.Cells[0].Paragraphs.First().Append("Id");
        //    r.Cells[1].Paragraphs.First().Append("Product Name");
        //    r.Cells[2].Paragraphs.First().Append("Product Price");
        //    r.Cells[3].Paragraphs.First().Append("Quantity");
        //    r.Cells[4].Paragraphs.First().Append("Price Total");
        //    r.Cells.ForEach(x => x.Paragraphs.First().Bold(true));


        //    foreach (Product product in Products)
        //    {
        //        r = waresTable.InsertRow();
        //        r.Cells[0].Paragraphs.First().Append(product.Id.ToString());
        //        r.Cells[1].Paragraphs.First().Append(product.ProductName);
        //        r.Cells[2].Paragraphs.First().Append(product.PricePerItem.ToString() + "$");
        //        r.Cells[3].Paragraphs.First().Append(product.Quantity.ToString());
        //        r.Cells[4].Paragraphs.First().Append(product.TotalPrice.ToString() + "$");
        //    }

        //    waresTable.Rows.ForEach(x => x.Cells.ForEach(y => y.Paragraphs.First().Alignment = Alignment.center));
        //    waresTable.SetColumnWidth(0, 30);
        //    waresTable.SetColumnWidth(3, 60);
        //    waresTable.Design = TableDesign.ColorfulList;

        //    waresTable.Alignment = Alignment.center;


        //    decimal totalPrice = 0;
        //    foreach (Product prod in Products)
        //    {
        //        totalPrice += prod.TotalPrice;
        //    }

        //    p = doc.InsertParagraph($"Total Cost :{totalPrice}$");
        //    p.Alignment = Alignment.right;
        //    p.LineSpacingBefore = 15;
        //    p.LineSpacingAfter = 30;
        //    p.FontSize(12);

        //    p = doc.InsertParagraph($"Document Generated By:\n{UserData.CreatorName}\n{UserData.CreatorEmail}\n" +
        //        $"Phone Number:{UserData.CreatorPhoneNumber}");

        //    p.Alignment = Alignment.left;
        //    p.FontSize(12);

        //    doc.Save();

        //    Process.Start("WINWORD.EXE", fileName);



        //}

        ///// <summary>
        ///// Function Generates new Document using file template 
        ///// </summary>
        ///// <param name="template">path to file template</param>
        ///// <param name="fileName">path to result file</param>
        ///// <param name="UserData">Data about Creator, Client and Company</param>
        ///// <param name="Products">List of products</param>
        //private static void GenerateDocumentFromTemplate(string template, string fileName,PersonalData  UserData, List<ItemModel> Products)
        //{
        //    File.Copy(template, fileName, true);
        //    string tableDescription = "PRODUCT_LIST_TABLE";

        //    DocX doc = DocX.Load(fileName);

        //    if (doc.FindAll("<CompanyInfo>").Count > 0)
        //    {
        //        doc.ReplaceText("<CompanyInfo>", UserData.CompanyData);
        //    }
        //    else
        //    {
        //        doc.ReplaceText("<CompanyName>", UserData.CompanyName);
        //        doc.ReplaceText("<CompanyEmail>", UserData.CompanyEmail);
        //        doc.ReplaceText("<CompanyAddress>", UserData.CompanyAddress);
        //        doc.ReplaceText("<CompanyPhoneNumber>", UserData.CompanyPhoneNumber);
        //    }

        //    if (doc.FindAll("<ClientInfo>").Count > 0)
        //    {
        //        doc.ReplaceText("<ClientInfo>", UserData.ClientData);
        //    }
        //    else
        //    {
        //        doc.ReplaceText("<ClientName>", UserData.ClientName);
        //        doc.ReplaceText("<ClientEmail>", UserData.ClientEmail);
        //        doc.ReplaceText("<ClientAddress>", UserData.ClientAddress);
        //        doc.ReplaceText("<ClientPhoneNumber>", UserData.ClientPhoneNumber);
        //    }

        //    if (doc.FindAll("<CreatorInfo>").Count > 0)
        //    {
        //        doc.ReplaceText("<CreatorInfo>", UserData.CreatorData);
        //    }
        //    else
        //    {
        //        doc.ReplaceText("<CreatorName>", UserData.CreatorName);
        //        doc.ReplaceText("<CreatorEmail>", UserData.CreatorEmail);
        //        doc.ReplaceText("<CreatorPhoneNumber>", UserData.CreatorPhoneNumber);
        //    }

        //    var dataTables = doc.Tables.Where(x => x.TableCaption == tableDescription);
        //    if (dataTables.Count() == 0)
        //    {
        //        throw new Exception("Couldn't find table with 'PRODUCT_LIST_TABLE' description!");
        //    }
        //    else
        //    {
        //        Table dataTable = dataTables.FirstOrDefault();


        //        //Remove empty Rows //TODO REFACTOR
        //        List<int> rowsToRemove = new List<int>();
        //        for (int i = 0; i < dataTable.RowCount; i++)
        //        {
        //            if (dataTable.Rows[i].Cells.Any(x => x.Paragraphs.FirstOrDefault().Text.Length < 1))
        //            {
        //                rowsToRemove.Add(i);
        //            }

        //        }

        //        for (int i = 0; i < rowsToRemove.Count; i++)
        //        {
        //            dataTable.RemoveRow(rowsToRemove[i]);
        //            for (int j = 0; j < rowsToRemove.Count; j++)
        //                rowsToRemove[j]--;
        //        }



        //        if (dataTable.RowCount > 1)
        //        {
        //            Row rowPattern = dataTable.Rows[1];

        //            foreach (var prod in Products)
        //            {
        //                // Insert a copy of the rowPattern at the last index in the table.
        //                var newItem = dataTable.InsertRow(rowPattern, dataTable.RowCount - 1);

        //                // Replace the default values of the newly inserted row.
        //                newItem.ReplaceText("<ProductName>", prod.ProductName);
        //                newItem.ReplaceText("<ProductId>", prod.Id.ToString());
        //                newItem.ReplaceText("<ProductPrice>", "$ " + prod.PricePerItem.ToString("N2"));
        //                newItem.ReplaceText("<ProductQuantity>", prod.Quantity.ToString());
        //                newItem.ReplaceText("<TotalPrice>", "$ " + prod.TotalPrice.ToString("N2"));
        //            }
        //            dataTable.RemoveRow();
        //        }
        //    }

        //    decimal totalPrice = Products.Sum(x => x.PricePerItem);
        //    doc.ReplaceText("<TotalPrice>", totalPrice.ToString("N2") + "$");

        //    doc.ReplaceText("<TodayDate>", DateTime.Now.ToLongDateString());



        //    doc.Save();
        //    Process.Start("WINWORD.EXE", fileName);


        //}

    }
}
