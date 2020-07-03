using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ClassLibrary
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
    }
}
