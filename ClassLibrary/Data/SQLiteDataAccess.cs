using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace ClassLibrary.Data
{
    // Class added to load ClientModel from database properly
    internal class TmpClientModel
    {
        public string NIP { get; set; }
        public string Street { get; set; }
        public string PostalCode{ get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CompanyName{ get; set; }

    }

    public class SQLiteDataAccess
    {
        public static List<ClientModel> LoadClients()
        {

            string sql = "select * from Clients";

            List<ClientModel> result = new List<ClientModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TmpClientModel>(sql, new DynamicParameters());
                if (output != null)
                {
                    foreach (TmpClientModel model in output)
                    {
                        result.Add(new ClientModel(model.NIP, model.PostalCode, model.City, model.Street, model.EmailAddress, model.CompanyName, model.Name, model.LastName, model.PhoneNumber));
                    }

                    return result;
                    
                }
                return new List<ClientModel>();
            }

        }

        public static void SaveClient(ClientModel client)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                if (client.IsValid)
                {
                    var output = cnn.Query<object>($"SELECT * FROM Clients WHERE " +
                        $"FullName='{client.FullName}' AND " +
                        $"((EmailAddress='{client.EmailAddress.Address}' AND EmailAddress!='') OR " +
                        $"(NIP='{client.NIP.Number}' AND NIP!='') OR " +
                        $"(Street='{client.Address.Street}' AND City='{client.Address.City}') OR " +
                        $"(PhoneNumber='{client.PhoneNumber.Number}' AND PhoneNumber!=''))",
                        new DynamicParameters());
                    if (!output.Any())
                    {
                        var obj = new
                        {
                            Name = client.Name,
                            LastName = client.LastName,
                            FullName = client.FullName,  
                            PhoneNumber = client.PhoneNumber.Number,
                            EmailAddress = client.EmailAddress.Address,
                            Street = client.Address.Street,
                            City = client.Address.City,
                            PostalCode = client.Address.PostalCode.Number,
                            NIP = client.NIP.Number,
                            CompanyName = client.CompanyName
                        };
                        cnn.Execute("insert into Clients (NIP, PostalCode, Street, City, PhoneNumber, EmailAddress, Name, LastName, FullName, CompanyName)" +
                            " values (@NIP, @PostalCode, @Street, @City, @PhoneNumber, @EmailAddress,@Name,@LastName, @FullName, @CompanyName)", obj);
                    }
                }
            }
        }

        public static void RemoveClient(ClientModel client)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute($"DELETE FROM Clients WHERE FullName='{client.FullName}' AND " +
                        $"Street='{client.Address.Street}' AND City='{client.Address.City}' AND " +
                        $"PhoneNumber='{client.PhoneNumber.Number}' AND EmailAddress='{client.EmailAddress.Address}'" +
                        $" AND NIP='{client.NIP.Number}'",
                        new DynamicParameters());
            }
        }


        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
