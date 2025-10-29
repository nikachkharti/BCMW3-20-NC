using TinyBank.Repository.Models;

namespace TinyBank.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();
        Customer GetSingleCustomer(int id);
        int AddCustomer(Customer newCustomer);
        int UpdateCustomer(Customer newCustomer);
        int DeleteCustomer(int id);
    }
}
