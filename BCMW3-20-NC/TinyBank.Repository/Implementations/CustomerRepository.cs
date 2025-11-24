using System.Text;
using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;
using TinyBank.Repository.Models.Enums;

namespace TinyBank.Repository.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _filePath;
        private readonly List<Customer> _customers;

        private CustomerRepository(string filePath, List<Customer> customers)
        {
            _filePath = filePath;
            _customers = customers;
        }

        //Factroy
        public static async Task<CustomerRepository> CreateAsync(string filePath)
        {
            var customers = new List<Customer>();

            await foreach (var customer in LoadDataAsync(filePath))
                customers.Add(customer);

            return new CustomerRepository(filePath, customers);
        }

        public List<Customer> GetCustomers() => _customers;
        public Customer GetSingleCustomer(int id) => _customers.FirstOrDefault(c => c.Id == id);
        public async Task<int> AddCustomerAsync(Customer newCustomer)
        {
            newCustomer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
            _customers.Add(newCustomer);
            await SaveDataAsync();

            return newCustomer.Id;
        }
        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);

            _customers.Remove(customer);
            await SaveDataAsync();

            return customer.Id;
        }
        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);

            if (index >= 0)
            {
                _customers[index] = customer;
                await SaveDataAsync();
            }

            return customer.Id;
        }


        #region HELPERS

        //წაკითხვა
        private static async IAsyncEnumerable<Customer> LoadDataAsync(string filePath)
        {
            if (!File.Exists(filePath))
                yield break;

            //FileStream კითხულობს მონაცემებს buffer - ებად, ანუ ნაწილ - ნაწილ
            using var fs = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 4096,//4KB default ზომა,
                    useAsync: true);

            using var reader = new StreamReader(fs);

            bool headerSkipped = false;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!headerSkipped)
                {
                    headerSkipped = true;
                    continue; //skip header
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue; //skip line

                var customer = FromCsv(line);

                if (customer != null)
                    yield return customer;
            }
        }


        private static Customer FromCsv(string customer)
        {
            var separatedCustomer = customer
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            Customer result = new();

            if (separatedCustomer.Length != 6)
                throw new FormatException("Customer format is invalid");

            result.Id = int.Parse(separatedCustomer[0]);
            result.Name = separatedCustomer[1];
            result.IdentityNumber = separatedCustomer[2];
            result.PhoneNumber = separatedCustomer[3];
            result.Email = separatedCustomer[4];
            result.CustomerType = Enum.Parse<CustomerType>(separatedCustomer[5]);

            return result;
        }


        //ჩაწერა
        private async Task SaveDataAsync()
        {
            //FileStream კითხულობს მონაცემებს buffer - ებად, ანუ ნაწილ - ნაწილ
            using var fs = new FileStream(
                    _filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,//4KB default ზომა,
                    useAsync: true);

            using var writer = new StreamWriter(fs, Encoding.UTF8);

            //header
            await writer.WriteAsync("Id,Name,IdentityNumber,PhoneNumber,Email,CustomerType");

            foreach (var customer in _customers)
                await writer.WriteAsync(ToCsv(customer));

            await writer.FlushAsync();
        }

        private static string ToCsv(Customer customer) => $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{Convert.ToInt32(customer.CustomerType)}".Trim();

        #endregion

    }
}
