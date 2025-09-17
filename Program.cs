using MyDBAccessor;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace GenericDBAccessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "server=localhost;port=3307;pwd=;uid=root;database=webshop";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM product", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["id"]} {reader["name"]} {reader["price"]}");
                        }
                    }
                }
            }

            DBAccessor dbAccessor = new DBAccessor();
        }
    }
}
