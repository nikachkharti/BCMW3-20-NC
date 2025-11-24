using System.Diagnostics.CodeAnalysis;
using TinyBank.Repository.Attributes;

namespace TinyBank.Repository.Models
{
    [DtoTransformable]
    public class Account
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
        public string Destination { get; set; }
    }

    public class AccountEquilityComparer : IEqualityComparer<Account>
    {
        public bool Equals(Account x, Account y) => x.Id == y.Id &&
                x.Iban.Trim().ToLower() == y.Iban.Trim().ToLower() &&
                x.Currency.Trim().ToLower() == y.Currency.Trim().ToLower() &&
                x.Balance == y.Balance &&
                x.CustomerId == y.CustomerId;

        public int GetHashCode([DisallowNull] Account obj) => obj.Id;
    }
}
