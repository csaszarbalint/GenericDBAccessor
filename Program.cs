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
                {
                    Console.WriteLine("ReadAll: \n" +
                    BookAccessor.Read()
                        .Aggregate("", (acc, obj) =>
                        {
                            return acc += $"{obj.Id} {obj.Title} {obj.NumberOfPages}\n";
                        })
                    );
                }

                Console.ReadKey();

                //read 
                {
                    Console.WriteLine("Read: " + BookAccessor.Read(10));
                }

                Console.ReadKey();

                //create
                {
                    Console.WriteLine("Create: " + BookAccessor.Create(
                        new Book()
                        {

                            Title = "New Book",
                            AuthorId = 1,
                            NumberOfPages = 123,
                            PublishingYear = 2024
                        }));
                }

                Console.ReadKey();

                //update
                {
                    Console.WriteLine("Updated: " + BookAccessor.Update(58,
                        new Book()
                        {
                            Id = 58,
                            Title = "Updated Book",
                            AuthorId = 1,
                            NumberOfPages = 321,
                            PublishingYear = 1988
                        }));
                }

                Console.ReadKey();

                //delete
                {
                    Console.WriteLine("Deleted: " + BookAccessor.Delete(58));
                }
            }
        }
    }
}
