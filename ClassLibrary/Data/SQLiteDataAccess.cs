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
                var output = cnn.Query<ClientMVCModel>(sql, new DynamicParameters());

                if (output != null)
                {
                    foreach (ClientMVCModel model in output)
                    {
                        result.Add(new ClientModel(model.Id, model.NIP, model.PostalCode, model.City, model.Street, model.EmailAddress, model.CompanyName, model.Name, model.LastName, model.PhoneNumber));
                    }

                    return result;
                    
                }
                return new List<ClientModel>();
            }

        }

        public static List<CreatorMVCModel> LoadCreators()
        {
            string sql = "select * from Creators";

            List<CreatorMVCModel> result = new List<CreatorMVCModel>();
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                var output = cnn.Query<CreatorMVCModel>(sql, new DynamicParameters()).ToList();
                if (output != null)
                {
                    return output;
                }
                else
                {
                    return new List<CreatorMVCModel>();
                }
            }
        }

        public static void SaveProduct(ItemMVCModel item)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (item != null)
                {
                    
                        var obj = new
                        {
                            Cost = item.Cost,
                            ItemName = item.ItemName,
                            Description  = item.Description,
                            ItemUnit = item.ItemUnit
                        };
                        cnn.Execute("insert into Products (Cost, ItemName, Description, ItemUnit)" +
                            " values (@Cost, @ItemName, @Description, @ItemUnit)", obj);
                        item.Id = cnn.Query<int>("select Id from Products").OrderBy(x => x).Last();
                    
                }
            }
        }

        public static void EditProduct(ItemMVCModel item)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (item != null)
                {
                    List<ItemMVCModel> products = LoadProducts();
                    if (products.Where(x => x.Id == item.Id).FirstOrDefault() != null)
                    {
                        var obj = new
                        {
                            Cost = item.Cost,
                            ItemName = item.ItemName,
                            Description = item.Description,
                            ItemUnit = item.ItemUnit,
                            Id = item.Id
                        };
                        cnn.Execute("UPDATE Products SET " +
                            "Cost = @Cost, ItemName = @ItemName, " +
                            "Description = @Description, ItemUnit = @ItemUnit " +
                            "WHERE Id = @Id", obj);

                    }
                }
            }
        }

        public static void RemoveProduct(ItemMVCModel item)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (item != null)
                {
                    List<ItemMVCModel> products = LoadProducts();
                    if (products.Where(x => x.Id == item.Id).FirstOrDefault() != null)
                    {
                        cnn.Execute($"DELETE FROM Products WHERE Id = {item.Id}");

                    }
                }
            }
        }

        public static List<ItemMVCModel> LoadProducts()
        {
            string sql = "select * from Products";

            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                List<ItemMVCModel> output = cnn.Query<ItemMVCModel>(sql, new DynamicParameters()).ToList();


                if (output != null)
                {
                    return output;
                }
                else
                {
                    return new List<ItemMVCModel>();
                }


            }
        }

        public static void SaveCreator(CreatorMVCModel commissionCreator)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (commissionCreator != null)
                {

                    var obj = new
                    {
                        Name = commissionCreator.Name,
                        LastName = commissionCreator.LastName,
                        PhoneNumber = commissionCreator.PhoneNumber,
                        EmailAddress = commissionCreator.EmailAddress
                    };
                    cnn.Execute("insert into Creators (Name, LastName, PhoneNumber, EmailAddress)" +
                        " values (@Name, @LastName, @PhoneNumber, @EmailAddress)", obj);
                    commissionCreator.Id = cnn.Query<int>("select Id from Creators").OrderBy(x => x).Last();

                }
            }
        }

        public static List<ClientMVCModel> LoadMVCClients()
        {

            string sql = "select * from Clients";

            List<ClientMVCModel> result = new List<ClientMVCModel>();
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                List<ClientMVCModel> output = cnn.Query<ClientMVCModel>(sql, new DynamicParameters()).ToList();


                if (output != null)
                {
                    return output;
                }
                else
                {
                    return new List<ClientMVCModel>();
                }
               

            }

        }

        public static List<CompanyMVCModel> LoadCompanies()
        {
            string sql = "select * from Companies";

            List<CompanyModel> result = new List<CompanyModel>();

            string connstring = ConfigurationManager.ConnectionStrings["DBFilepath"].ConnectionString;
            
            using (IDbConnection cnn = new SQLiteConnection(connstring))
            {
                List<CompanyMVCModel> output = cnn.Query<CompanyMVCModel>(sql, new DynamicParameters()).ToList();
                return output;
            }
        }

        public static void SaveCompany(CompanyMVCModel company)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (company != null)
                {
                    var o = cnn.Query<object>($"SELECT * FROM Companies WHERE " +
                        $"CompanyName='{company.CompanyName}' AND " +
                        $"((EmailAddress='{company.EmailAddress}' AND EmailAddress!='') OR " +
                        $"(NIP='{company.NIP}' AND NIP!='') OR " +
                        $"(Street='{company.Street}' AND City='{company.City}') OR " +
                        $"(PhoneNumber='{company.PhoneNumber}' AND PhoneNumber!=''))",
                        new DynamicParameters());
                    if (!o.Any())
                    {
                        var obj = new
                        {
                            PhoneNumber = company.PhoneNumber,
                            EmailAddress = company.EmailAddress,
                            Street = company.Street,
                            City = company.City,
                            PostalCode = company.PostalCode,
                            NIP = company.NIP,
                            CompanyName = company.CompanyName,
                            REGON = company.REGON,
                        };

                       
                        cnn.Execute("insert into Companies (NIP, PostalCode, Street, City, PhoneNumber, EmailAddress, CompanyName, REGON)" +
                            " values (@NIP, @PostalCode, @Street, @City, @PhoneNumber, @EmailAddress, @CompanyName, @REGON)", obj);

                       //If storing company into db was succesful, return new record's Id
                       company.Id = cnn.Query<int>("select Id from Companies").OrderBy(x => x).Last();
                    }
                }
            }
        }

        public static void EditClient(ClientMVCModel client)
        {
                using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
                {
                    if (client != null)
                    {
                        List<ClientModel> clients = LoadClients();
                        if (clients.Where(x => x.Id == client.Id).FirstOrDefault() != null)
                        {
                            var obj = new
                            {
                                PhoneNumber = client.PhoneNumber,
                                EmailAddress = client.EmailAddress,
                                Street = client.Street,
                                City = client.City,
                                PostalCode = client.PostalCode,
                                NIP = client.NIP,
                                CompanyName = client.CompanyName,
                                Name = client.Name,
                                LastName = client.LastName,
                                FullName = client.FullName,
                                Id = client.Id
                            };
                            cnn.Execute("UPDATE Clients SET " +
                                "NIP = @NIP, PostalCode = @PostalCode, " +
                                "Street = @Street, City = @City, " +
                                "PhoneNumber = @PhoneNumber, EmailAddress = @EmailAddress, " +
                                "CompanyName = @CompanyName, Name = @Name, " +
                                "LastName = @LastName, FullName = @FullName " +
                                "WHERE Id = @Id", obj);

                        }
                    }
                }
            }


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
                        client.Id = cnn.Query<int>("select Id from Clients").OrderBy(x => x).Last(); 
                    }
                }
            }
        }

        public static void SaveClient(ClientMVCModel client)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                
                var output = cnn.Query<object>($"SELECT * FROM Clients WHERE " +
                    $"FullName='{client.FullName}' AND " +
                    $"((EmailAddress='{client.EmailAddress}' AND EmailAddress!='') OR " +
                    $"(NIP='{client.NIP}' AND NIP!='') OR " +
                    $"(Street='{client.Street}' AND City='{client.City}') OR " +
                    $"(PhoneNumber='{client.PhoneNumber}' AND PhoneNumber!=''))",
                    new DynamicParameters());
                if (!output.Any())
                {
                    if (client.Company == true)
                    {
                        var obj = new
                        {

                            Name = client.Name,
                            LastName = client.LastName,
                            PhoneNumber = client.PhoneNumber,
                            EmailAddress = client.EmailAddress,
                            Street = client.Street,
                            City = client.City,
                            PostalCode = client.PostalCode,
                            NIP = client.NIP,
                            CompanyName = client.CompanyName,
                            FullName = client.FullName
                        };
                        cnn.Execute("insert into Clients (NIP, PostalCode, Street, City, PhoneNumber, EmailAddress, Name, LastName, FullName, CompanyName)" +
                            " values (@NIP, @PostalCode, @Street, @City, @PhoneNumber, @EmailAddress,@Name,@LastName, @FullName, @CompanyName)", obj);
                        client.Id = cnn.Query<int>("select Id from Clients").OrderBy(x => x).Last();
                    }
                    else
                    {
                        var obj = new
                        {

                            Name = client.Name,
                            LastName = client.LastName,
                            PhoneNumber = client.PhoneNumber,
                            EmailAddress = client.EmailAddress,
                            Street = client.Street,
                            City = client.City,
                            PostalCode = client.PostalCode,
                            FullName = client.FullName
                        };
                        cnn.Execute("insert into Clients (PostalCode, Street, City, PhoneNumber, EmailAddress, Name, LastName, FullName)" +
                            " values (@PostalCode, @Street, @City, @PhoneNumber, @EmailAddress,@Name,@LastName, @FullName)", obj);
                        client.Id = cnn.Query<int>("select Id from Clients").OrderBy(x => x).Last();
                    }
                }
                    
                
            }
            
        }

        public static void EditCompany(CompanyMVCModel company)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                if (company != null)
                {
                    List<CompanyMVCModel> companies = LoadCompanies();
                    if (companies.Where(x => x.Id == company.Id).FirstOrDefault() != null)
                    {
                        var obj = new
                        {
                            PhoneNumber = company.PhoneNumber,
                            EmailAddress = company.EmailAddress,
                            Street = company.Street,
                            City = company.City,
                            PostalCode = company.PostalCode,
                            NIP = company.NIP,
                            CompanyName = company.CompanyName,
                            REGON = company.REGON,
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

        public static void RemoveClient(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.CnnString()))
            {
                cnn.Execute($"DELETE FROM Clients WHERE Id={id}",
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
