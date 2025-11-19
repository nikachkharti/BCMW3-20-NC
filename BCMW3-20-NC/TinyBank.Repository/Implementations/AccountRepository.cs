using System.Text;
using System.Text.Json;
using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;

namespace TinyBank.Repository.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _filePath;
        private readonly List<Account> _accounts;

        private AccountRepository(string filePath, List<Account> accounts)
        {
            _filePath = filePath;
            _accounts = accounts;
        }


        /// <summary>
        /// Factroy method async constructor
        /// </summary>
        public static async Task<AccountRepository> CreateAsync(string filePath)
        {
            var accounts = new List<Account>();

            if (File.Exists(filePath))
            {
                using var fs = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 8192,
                    useAsync: true);

                using var ms = new MemoryStream();
                await fs.CopyToAsync(ms);

                ms.Position = 0;
                string json = Encoding.UTF8.GetString(ms.ToArray());

                var deserialized = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (deserialized != null)
                    accounts.AddRange(deserialized);
            }

            return new AccountRepository(filePath, accounts);
        }

        public List<Account> GetAccounts() => _accounts;
        public Account GetSingleAccount(int id)
            => _accounts.FirstOrDefault(a => a.Id == id);
        public List<Account> GetAccountsOfCustomer(int customerId)
            => _accounts.Where(a => a.CustomerId == customerId).ToList();
        public async Task<int> AddAccountAsync(Account newAccount)
        {
            newAccount.Id = _accounts.Any() ? _accounts.Max(a => a.Id) + 1 : 1;
            _accounts.Add(newAccount);
            await SaveDataAsync();
            return newAccount.Id;
        }
        public async Task<int> DeleteAccountAsync(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
                return -1;

            _accounts.Remove(account);
            await SaveDataAsync();
            return account.Id;
        }
        public async Task<int> UpdateAccountAsync(Account account)
        {
            var index = _accounts.FindIndex(a => a.Id == account.Id);
            if (index >= 0)
            {
                _accounts[index] = account;
                await SaveDataAsync();
            }
            return account.Id;
        }


        #region HELPERS

        private async Task SaveDataAsync()
        {
            var jsonPayload = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });

            using var fs = new FileStream(
                _filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 8192,
                useAsync: true);

            byte[] bytes = Encoding.UTF8.GetBytes(jsonPayload);
            await fs.WriteAsync(bytes, 0, bytes.Length);
            await fs.FlushAsync();
        }

        #endregion

    }
}
