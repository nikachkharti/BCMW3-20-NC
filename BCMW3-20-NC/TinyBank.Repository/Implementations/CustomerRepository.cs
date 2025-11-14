using System.Text;
using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;

namespace TinyBank.Repository.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _filePath;
        private readonly List<Customer> _customers;

        public CustomerRepository(string filePath)
        {
            _filePath = filePath;
            _customers = LoadData();
        }

        public List<Customer> GetCustomers() => _customers;
        public Customer GetSingleCustomer(int id) => _customers.FirstOrDefault(c => c.Id == id);
        public int AddCustomer(Customer newCustomer)
        {
            newCustomer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
            _customers.Add(newCustomer);
            SaveData();

            return newCustomer.Id;
        }
        public int DeleteCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);

            _customers.Remove(customer);
            SaveData();

            return customer.Id;
        }
        public int UpdateCustomer(Customer customer)
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);

            if (index >= 0)
            {
                _customers[index] = customer;
                SaveData();
            }

            return customer.Id;
        }


        #region HELPERS

        //წაკითხვა
        private List<Customer> LoadData()
        {
            var customers = new List<Customer>();

            if (!File.Exists(_filePath))
                return customers;

            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    ms.Position = 0;

                    var content = Encoding.UTF8.GetString(ms.ToArray());
                    var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 1; i < lines.Length; i++) // skip header
                    {
                        var customer = FromCsv(lines[i]);
                        if (customer != null)
                            customers.Add(customer);
                    }
                }
            }

            return customers;
        }
        private Customer FromCsv(string customer)
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
        private void SaveData()
        {
            // Prepare CSV content in memory
            var lines = new List<string>()
            {
                "Id,Name,IdentityNumber,PhoneNumber,Email,CustomerType"
            };

            foreach (var customer in _customers)
            {
                lines.Add(ToCsv(customer));
            }

            // Join lines with proper newlines
            string csvContent = string.Join(Environment.NewLine, lines);

            // Convert string to bytes (UTF-8)
            byte[] buffer = Encoding.UTF8.GetBytes(csvContent);

            // Overwrite the file with FileStream
            using (var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush(); // ensure data is written to disk
            }
        }

        private string ToCsv(Customer customer) => $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{Convert.ToInt32(customer.CustomerType)}".Trim();

        #endregion

    }
}
