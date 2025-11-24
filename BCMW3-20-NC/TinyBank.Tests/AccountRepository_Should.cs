using TinyBank.Repository.Implementations;
using TinyBank.Repository.Models;

namespace TinyBank.Tests
{
    [Trait("Category", "TinyBank")]
    public class AccountRepository_Should
    {
        private readonly string _testFilePath = @"../../../../TinyBank.Tests/Data/Accounts.json";

        [Fact]
        public async Task Get_All_Accounts()
        {
            // Arrange
            var repository = await AccountRepository.CreateAsync(_testFilePath);
            var expectedCount = 30;

            // Act
            var accounts = repository.GetAccounts();

            // Assert
            Assert.Equal(expectedCount, accounts.Count());
        }

        [Fact]
        public async Task Get_Empty_List_If_No_Accounts()
        {
            // Arrange
            var repository = await AccountRepository.CreateAsync(_testFilePath);
            var expectedCount = 0;

            // Act
            var accounts = repository.GetAccounts().ToArray();
            Array.Resize(ref accounts, 0);

            // Assert
            Assert.Equal(expectedCount, accounts.Length);
            Assert.Empty(accounts);
        }

        [Fact]
        public async Task Get_Single_Account()
        {
            // Arrange
            var repository = await AccountRepository.CreateAsync(_testFilePath);
            var expected = new Account()
            {
                Id = 1,
                Iban = "GE94SB5621487456325158",
                Currency = "USD",
                Balance = 6712.12m,
                CustomerId = 1,
                Destination = null
            };

            // Act
            var actual = repository.GetSingleAccount(1);

            // Assert
            Assert.Equal(expected, actual, new AccountEquilityComparer());
        }

        [Fact]
        public async Task Add_Account()
        {
            // Arrange
            var repository = await AccountRepository.CreateAsync(_testFilePath);

            var maxId = repository.GetAccounts().Max(x => x.Id);
            var newAccount = new Account()
            {
                Id = maxId + 1,
                Iban = "GE94SB5621487456325151",
                Currency = "GEL",
                Balance = 0.00m,
                CustomerId = 1,
                Destination = null
            };
            var expected = maxId + 1;

            // Act
            var addedId = await repository.AddAccountAsync(newAccount);
            var actualCount = repository.GetAccounts().Count;

            // Assert
            Assert.Equal(expected, addedId);
            Assert.Equal(expected, actualCount);
        }

        [Fact]
        public async Task Delete_Account()
        {
            // Arrange
            var repository = await AccountRepository.CreateAsync(_testFilePath);
            var accountToDelete = repository.GetAccounts().Max(a => a.Id);
            var expectedCount = repository.GetAccounts().Count - 1;

            // Act
            await repository.DeleteAccountAsync(accountToDelete);
            var actualCount = repository.GetAccounts().Count();

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task Update_Account()
        {
            // Arrange
            var repository = await AccountRepository.CreateAsync(_testFilePath);

            var updatedAccount = new Account()
            {
                Id = 23,
                Iban = "GE89SB5512359654475123",
                Currency = "GEL",
                Balance = 232.00m,
                CustomerId = 10,
                Destination = null
            };

            // Act
            await repository.UpdateAccountAsync(updatedAccount);
            var actual = repository.GetSingleAccount(23);

            // Assert
            Assert.Equal(updatedAccount, actual);
        }
    }
}
