using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TinyBank.Repository.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _filePath;
        private readonly List<Account> _accounts;

        public AccountRepository(string filePath)
        {
            _filePath = filePath;
            _accounts = LoadData();
        }

        public List<Account> GetAccounts() => _accounts;
        public Account GetSingleAccount(int id) => _accounts.FirstOrDefault(a => a.Id == id);
        public List<Account> GetAccountsOfCustomer(int customerId) => _accounts
                .Where(a => a.CustomerId == customerId)
                .ToList();
        public int AddAccount(Account newAccount)
        {
            newAccount.Id = _accounts.Any() ? _accounts.Max(c => c.Id) + 1 : 1;
            _accounts.Add(newAccount);
            SaveData();

            return newAccount.Id;
        }
        public int DeleteAccount(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);

            _accounts.Remove(account);
            SaveData();

            return account.Id;
        }
        public int UpdateAccount(Account account)
        {
            var index = _accounts.FindIndex(a => a.Id == account.Id);

            if (index >= 0)
            {
                _accounts[index] = account;
                SaveData();
            }

            return account.Id;
        }


        #region HELPERS

        //წაკითხვა
        private List<Account> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<Account>();

            using (FileStream fs = new(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.ReadExactly(buffer);
                string json = Encoding.UTF8.GetString(buffer);

                var accounts = FromJson(json);
                return accounts ?? new List<Account>();
            }
        }

        private List<Account> FromJson(string line) =>
            JsonSerializer.Deserialize<List<Account>>(line, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        //ჩაწერა
        private string ToJson(List<Account> accounts) =>
            JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });

        private void SaveData()
        {
            //File.WriteAllText(_filePath, ToJson(_accounts));
            var jsonPayload = ToJson(_accounts);

            using (FileStream fs = new(_filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(jsonPayload);
                fs.Write(bytes, 0, bytes.Length);
            }

        }

        #endregion

    }
}
