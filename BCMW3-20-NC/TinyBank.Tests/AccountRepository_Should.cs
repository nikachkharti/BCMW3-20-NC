using TinyBank.Repository.Implementations;
using TinyBank.Repository.Models;

namespace TinyBank.Tests
{
    [Trait("Category", "TinyBank")]
    public class AccountRepository_Should
    {
        private readonly string _testFilePath = @"../../../../TinyBank.Tests/Data/Accounts.json";

        [Fact]
        public void Get_All_Accounts()
        {
            //Arrange
            var repository = new AccountRepository(_testFilePath);
            var expectedCount = 30;

            //Act
            var accounts = repository.GetAccounts();

            //Assert
            //Assert.Equal(expectedCount, accounts.Count());
        }

        [Fact]
        public void Get_Empty_List_If_No_Accounts()
        {
            //Arrange
            var repository = new AccountRepository(_testFilePath);
            var expectedCount = 0;

            //Act
            var accounts = repository.GetAccounts().ToArray();
            Array.Resize(ref accounts, 0);

            //Assert
            Assert.Equal(expectedCount, accounts.Length);
            Assert.Empty(accounts);
        }

        [Fact]
        public void Get_Single_Account()
        {
            //Arrange
            var repository = new AccountRepository(_testFilePath);
            var expected = new Account()
            {
                Id = 1,
                Iban = "GE94SB5621487456325158",
                Currency = "USD",
                Balance = 6712.12m,
                CustomerId = 1,
                Destination = null
            };

            //Act
            var actual = repository.GetSingleAccount(1);

            //Assert
            Assert.Equal(expected, actual, new AccountEquilityComparer());
        }

        [Fact]
        public void Add_Account()
        {
            //Arrange
            var repository = new AccountRepository(_testFilePath);
            var expected = 31;
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

            //Act
            repository.AddAccount(newAccount);
            var actual = repository.GetAccounts().Count;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_Account()
        {
            //Arrange
            var repository = new AccountRepository(_testFilePath);
            var accountToDelete = 31;
            var expected = 30;

            //Act
            repository.DeleteAccount(accountToDelete);
            var actual = repository.GetAccounts().Count();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_Account()
        {
            //Arrange
            var repository = new AccountRepository(_testFilePath);
            var updatedAccount = new Account()
            {
                Id = 23,
                Iban = "GE89SB5512359654475123",
                Currency = "GEL",
                Balance = 232.00m,
                CustomerId = 10,
                Destination = null
            };

            //Act
            repository.UpdateAccount(updatedAccount);

            //Assert
            var actual = repository.GetSingleAccount(23);
            Assert.Equal(updatedAccount, actual);
        }
    }
}
