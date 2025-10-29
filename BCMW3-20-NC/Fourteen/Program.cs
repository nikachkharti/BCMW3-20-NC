using Algorithms.Models;
using System.Text.Json;

namespace Fourteen
{
    internal class Program
    {
        static void Main(string[] args)
        {

            /*
             
            +
            List<Customer> GetCustomers();
            Customer GetSingleCustomer(int id);
            int AddCustomer(Customer newCustomer);
            int UpdateCustomer(Customer newCustomer);
            int DeleteCustomer(int id);


            List<Account> GetAccounts();
            List<Account> GetAccountsOfCustomer(int customerId);
            Account GetSingleAccount(int id);
            int AddAccount(Account newAccount);
            int UpdateAccount(Account account);
            int DeleteAccount(int id);
             
             */








            //var path = @"../../../cars.json";
            //string jsonData = File.ReadAllText(path);

            ////დესერიალიზაცია -> Json - ის ობიექტის C# - ის ობიექტად გადაქცევას
            ////სერიალიზაცია -> C# - ის ობიექტის, Json ობიექტად გადაქცევას

            //List<Vehicle>? vehicles = JsonSerializer.Deserialize<List<Vehicle>>(jsonData);
            //string vehiclesAsString = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions { WriteIndented = true });

            //Console.WriteLine(vehiclesAsString);


            //File.WriteAllText(@"../../../cars2.json", vehiclesAsString);
        }
    }
}
