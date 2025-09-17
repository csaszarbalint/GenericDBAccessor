using MyDBAccessor;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace GenericDBAccessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            DbConnection connection = new MySqlConnection();

            DBAccessor dbAccessor = new DBAccessor();
        }
    }
}
