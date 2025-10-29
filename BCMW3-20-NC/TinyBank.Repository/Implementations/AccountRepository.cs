using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;

namespace TinyBank.Repository.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        public int AddAccount(Account newAccount)
        {
            throw new NotImplementedException();
        }

        public int DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAccountsOfCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public Account GetSingleAccount(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateAccount(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
