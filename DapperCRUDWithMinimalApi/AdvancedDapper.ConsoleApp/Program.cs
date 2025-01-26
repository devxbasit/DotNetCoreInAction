// See https://aka.ms/new-console-template for more information


using System.Data;
using AdvancedDapper.ConsoleApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AdvancedDapper.ConsoleApp
{
    class Program
    {
        private static readonly string _connectionString;

        static Program()
        {
            _connectionString = "Data Source = localhost,1433; Initial Catalog = DapperCRUDWithMinimalApiDb; Integrated Security = false; User Id = sa; Password = strongPA55WORD!; TrustServerCertificate = true";
        }

        public static void Main(string[] args)
        {
            MapMultipleObjects();
            Console.ReadKey();
        }

        public static async void MapMultipleObjects()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"select pe.*, ph.* 
                               from dbo.Person pe
                               left join dbo.Phone ph
                                 on pe.CellPhoneId = ph.Id;";

                var people = connection.Query<FullPersonModel, PhoneModel, FullPersonModel>(sql,
                    (person, phone) =>
                    {
                        person.CellPhone = phone;
                        return person;
                    });

                foreach (var p in people)
                {
                    Console.WriteLine($" ${p.FirstName} {p.LastName}: Cell: {p.CellPhone?.PhoneNumber}");
                }
            }
        }
    }
}
