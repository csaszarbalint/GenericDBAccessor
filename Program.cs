using GenericDBAccessor.Models;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace GenericDBAccessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "server=localhost;port=3307;pwd=;uid=root;database=books";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                DBAccessor<Book> BookAccessor = new DBAccessor<Book>(conn);

                //read all
                Console.WriteLine(
                    BookAccessor.Read()
                    .Aggregate("", (acc, obj) =>
                    {
                        return acc += $"{obj.Id} {obj.Title} {obj.NumberOfPages}\n";
                    })
                );

                //read 
                Console.WriteLine(BookAccessor.Read(10));

                //create
                Console.WriteLine(BookAccessor.Create(
                    new Book()
                    {
                        Id = 3,
                        Title = "New Book",
                        AuthorId = 1,
                        NumberOfPages = 123,
                        PublishingYear = 2024
                    }));
            }
        }
    }
}
