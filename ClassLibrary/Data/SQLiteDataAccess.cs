using ClassLibrary.DataModels;
using ClassLibrary.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace ClassLibrary.Data
{
    // Class added to load ClientModel from database properly


    public class SQLiteDataAccess
    {
        public static List<ClientModel> LoadClients()
        {

            string sql = "select * from Clients";

            List<ClientModel> result = new List<ClientModel>();
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                var output = cnn.Query<DBClientModel>(sql, new DynamicParameters());
                if (output != null)
                {
                    foreach (DBClientModel model in output)
                    {
                        result.Add(new ClientModel(model.id, model.NIP, model.PostalCode, model.City, model.Street, model.EmailAddress, model.CompanyName, model.Name, model.LastName, model.PhoneNumber));
                    }

                    return result;
                    
                }
                return new List<ClientModel>();
            }

        }

        public static List<CompanyModel> LoadCompanies()
        {
            string sql = "select * from Companies";

            List<CompanyModel> result = new List<CompanyModel>();

            string connstring = ConfigurationManager.ConnectionStrings["DBFilepath"].ConnectionString;
            
            using (IDbConnection cnn = new SQLiteConnection(connstring))
            {
                var output = cnn.Query<DBCompanyModel>(sql, new DynamicParameters());
                if (output != null)
                {
                    foreach (DBCompanyModel model in output)
                    {
                        result.Add(new CompanyModel(model.Id,model.NIP, model.PostalCode, model.City, model.Street, model.EmailAddress, model.CompanyName, model.PhoneNumber, model.REGON));
                    }

                    return result;

                }
                return new List<CompanyModel>();
            }
        }

        //TODO ADD ID
        public static void SaveCompany(CompanyModel company)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (company != null)
                {
                    var o = cnn.Query<object>($"SELECT * FROM Companies WHERE " +
                        $"CompanyName='{company.CompanyName}' AND " +
                        $"((EmailAddress='{company.EmailAddress.Address}' AND EmailAddress!='') OR " +
                        $"(NIP='{company.NIP.Number}' AND NIP!='') OR " +
                        $"(Street='{company.Address.Street}' AND City='{company.Address.City}') OR " +
                        $"(PhoneNumber='{company.PhoneNumber.Number}' AND PhoneNumber!=''))",
                        new DynamicParameters());
                    if (!o.Any())
                    {
                        var obj = new
                        {
                            PhoneNumber = company.PhoneNumber.Number,
                            EmailAddress = company.EmailAddress.Address,
                            Street = company.Address.Street,
                            City = company.Address.City,
                            PostalCode = company.Address.PostalCode.Number,
                            NIP = company.NIP.Number,
                            CompanyName = company.CompanyName,
                            REGON = company.REGON.Number,
                        };

                       
                        cnn.Execute("insert into Companies (NIP, PostalCode, Street, City, PhoneNumber, EmailAddress, CompanyName, REGON)" +
                            " values (@NIP, @PostalCode, @Street, @City, @PhoneNumber, @EmailAddress, @CompanyName, @REGON)", obj);

                       //If storing company into db was succesful, return new record's Id
                       company.Id = cnn.Query<int>("select Id from Companies").OrderBy(x => x).Last();
                    }
                }
            }
        }

        //TODO ADD ID
        public static void SaveClient(ClientModel client)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
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

        public static void EditCompany(CompanyModel company)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (company != null)
                {
                    List<CompanyModel> companies = LoadCompanies();
                    if (companies.Where(x => x.Id == company.Id).FirstOrDefault() != null)
                    {
                        var obj = new
                        {
                            PhoneNumber = company.PhoneNumber.Number,
                            EmailAddress = company.EmailAddress.Address,
                            Street = company.Address.Street,
                            City = company.Address.City,
                            PostalCode = company.Address.PostalCode.Number,
                            NIP = company.NIP.Number,
                            CompanyName = company.CompanyName,
                            REGON = company.REGON.Number,
                            Id = company.Id
                        };
                        cnn.Execute("UPDATE Companies SET " +
                            "NIP = @NIP, PostalCode = @PostalCode, " +
                            "Street = @Street, City = @City, " +
                            "PhoneNumber = @PhoneNumber, EmailAddress = @EmailAddress, " +
                            "CompanyName = @CompanyName, REGON = @REGON " +
                            "WHERE Id = @Id",obj);

                    }
                }
            }
        }

        //TODO REFACTOR 
        public static void RemoveClient(ClientModel client)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                cnn.Execute($"DELETE FROM Clients WHERE FullName='{client.FullName}' AND " +
                        $"Street='{client.Address.Street}' AND City='{client.Address.City}' AND " +
                        $"PhoneNumber='{client.PhoneNumber.Number}' AND EmailAddress='{client.EmailAddress.Address}'" +
                        $" AND NIP='{client.NIP.Number}'",
                        new DynamicParameters());
            }
        }

        public static void RemoveCompany(int id)
        {
            using(IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                cnn.Execute($"DELETE FROM Companies WHERE Id = {id}");
            }
        }


    }
}
