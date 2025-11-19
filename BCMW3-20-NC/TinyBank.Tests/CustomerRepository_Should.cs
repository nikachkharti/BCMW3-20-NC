using System.Reflection;
using TinyBank.Repository.Implementations;
using TinyBank.Repository.Models;

namespace TinyBank.Tests
{
    [Trait("Category", "TinyBank")]
    public class CustomerRepository_Should
    {
        private readonly string _testFilePath = @"../../../../TinyBank.Tests/Data/Customers.csv";

        [Fact]
        public async Task Get_Multiple_Customers()
        {
            // Arrange
            var repository = await CustomerRepository.CreateAsync(_testFilePath);
            var expectedCount = 10;

            // Act
            var customers = repository.GetCustomers();

            // Assert
            Assert.Equal(expectedCount, customers.Count());
        }

        [Fact]
        public async Task Get_Individual_Customer()
        {
            // Arrange
            var repository = await CustomerRepository.CreateAsync(_testFilePath);
            var expected = new Customer()
            {
                Id = 1,
                Name = "Iakob Qobalia",
                PhoneNumber = "555337681",
                Email = "Iakob.Qobalia@gmail.com",
                IdentityNumber = "31024852345",
                CustomerType = CustomerType.Phyisical
            };

            // Act
            var actual = repository.GetSingleCustomer(1);

            // Assert
            Assert.Equal(expected, actual, new CustomerEqulityComparer());
        }

        [Fact]
        public async Task Add_Customer()
        {
            // Arrange
            var repository = await CustomerRepository.CreateAsync(_testFilePath);

            var expected = 11;
            var maxId = repository.GetCustomers().Max(x => x.Id);

            var newCustomer = new Customer()
            {
                Id = maxId + 1,
                Name = "Nikoloz Chkhartishvili",
                PhoneNumber = "558490588",
                Email = "Nikoloz.Chkhartishvili@gmail.com",
                IdentityNumber = "01024085083",
                CustomerType = CustomerType.Phyisical
            };

            // Act
            await repository.AddCustomerAsync(newCustomer);

            var actual = repository.GetCustomers().Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Update_Customer()
        {
            // Arrange
            var repository = await CustomerRepository.CreateAsync(_testFilePath);

            var updatedCustomer = new Customer()
            {
                Id = 3,
                Name = "Zaal Chkhartishvili",
                PhoneNumber = "558490588",
                Email = "Zaal.Chkhartishvili@gmail.com",
                IdentityNumber = "01024085083",
                CustomerType = CustomerType.Phyisical
            };

            // Act
            await repository.UpdateCustomerAsync(updatedCustomer);

            // Assert
            var actual = repository.GetSingleCustomer(3);
            Assert.Equal(updatedCustomer, actual);
        }

        [Fact]
        public async Task Delete_Customer()
        {
            // Arrange
            var repository = await CustomerRepository.CreateAsync(_testFilePath);

            var customerIdToDelete = 11;
            var expected = 10;

            // Act
            await repository.DeleteCustomerAsync(customerIdToDelete);

            var actual = repository.GetCustomers().Count();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
