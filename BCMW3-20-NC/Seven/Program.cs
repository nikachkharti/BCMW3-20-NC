using System.Runtime.CompilerServices;

namespace Seven
{
    //public sealed class X
    //{
    //}

    //class Y : X
    //{
    //}

    public abstract class Client
    {
        private string identifier;
        public string Identifier
        {
            get { return identifier; }
            set
            {
                if (value.Length == 11)
                {
                    identifier = value;
                }
                else
                {
                    throw new ArgumentException("Invalid personal number");
                    throw new ArgumentException("Invalid personal number");
                }
            }
        }

        public Account Account { get; set; }

        public Client(string identifier, Account account)
        {
            Identifier = identifier;
            Account = account;
        }

        public virtual void IntroduceYourself()
        {
            Console.WriteLine($"Hello, my identifier is {identifier}, I have balance {Account.Balance}");
        }

        public abstract void Withdrdaw(decimal amount);


        //შედარებით იშვიათად გამოყენებადი
        //public void Withdraw(decimal amount)
        //{
        //    if (Account.Balance >= amount)
        //    {
        //        Account.Balance -= amount;
        //    }
        //}
    }


    public class PhyisicalClient : Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PhyisicalClient(string identifier, Account account, string firstName, string lastName) : base(identifier, account)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override void IntroduceYourself()
        {
            Console.WriteLine($"Gamarjoba me mqvia {FirstName} {LastName}");
        }

        public override void Withdrdaw(decimal amount)
        {
            if (amount < 2000 && Account.Balance >= amount)
            {
                Account.Balance -= amount;
            }
        }
    }

    public class LegalClient : Client
    {
        public string CompanyName { get; set; }

        public LegalClient(string identifier, Account account) : base(identifier, account)
        {
        }

        public override void IntroduceYourself()
        {
            Console.WriteLine($"Zdarova me mqvia {CompanyName}");
        }

        public override void Withdrdaw(decimal amount)
        {
            if (Account.Balance >= amount)
            {
                Account.Balance -= amount;
            }
        }
    }


    public class Account
    {
        private string accountNumber;
        private string currency;
        private decimal balance;

        public decimal Balance
        {
            get { return balance; }
            set
            {
                if (value > 0)
                {
                    balance = value;
                }
            }
        }
        public string Currency
        {
            get { return currency; }
            set
            {
                if (value.Length == 3)
                {
                    currency = value;
                }
                else
                {
                    throw new ArgumentException("Invalid currency value");
                }
            }
        }
        public string AccountNumber
        {
            get { return accountNumber; }
            set
            {
                if (value.Length == 22)
                {
                    accountNumber = value;
                }
                else
                {
                    throw new ArgumentException("Invalid account number");
                }
            }
        }
    }




    internal class Program
    {
        static void Main(string[] args)
        {

            PhyisicalClient p = new(
            "12345678901",
            new Account()
            {
                AccountNumber = "GE29TB0000000000000001",
                Balance = 1500.00m,
                Currency = "USD"
            },
            "John",
            "Doe"
            );
            PhyisicalClient p2 = new(
            "12345678901",
            new Account()
            {
                AccountNumber = "GE29TB0000000000000001",
                Balance = 1500.00m,
                Currency = "USD"
            },
            "John",
            "Doe"
            );
            PhyisicalClient p3 = new(
            "12345678901",
            new Account()
            {
                AccountNumber = "GE29TB0000000000000001",
                Balance = 1500.00m,
                Currency = "USD"
            },
            "John",
            "Doe"
            );

            p3.Withdrdaw(1900);


            PhyisicalClient[] phyisicalClients = [p, p2, p3];

            p.IntroduceYourself();
            Console.WriteLine(LogActivity(p));


            LegalClient l = new("123456789RC", new Account()
            {
                AccountNumber = "GE29TB0000000000000002",
                Currency = "EUR",
                Balance = 10000m
            });
            l.Identifier = "123456789RC";
            l.CompanyName = "Test CO";

            LegalClient l2 = new("123456789RC", new Account()
            {
                AccountNumber = "GE29TB0000000000000002",
                Currency = "EUR",
                Balance = 10000m
            });
            l.Identifier = "123456789RC";
            l.CompanyName = "Test CO";

            LegalClient l3 = new("123456789RC", new Account()
            {
                AccountNumber = "GE29TB0000000000000002",
                Currency = "EUR",
                Balance = 10000m
            });
            l.Identifier = "123456789RC";
            l.CompanyName = "Test CO";

            LegalClient[] legalClients = [l, l2, l3];


            l.IntroduceYourself();
            Console.WriteLine(LogActivity(l));



            //1. ენკაფსულაცია +
            //2. მემკვიდრეობა = პროგრამაში ზოგადის და კონკრეტულის ცნების შემოტანას (კოდის დუბლირება, განზოგადებული კოდის წერა)
            //3. პოლიმორფიზმი = ერთი კონკრეტული მეთოდს მივცე სხვადასხვა ფორმა გამომდინარე იქიდან თუ რომელი ობიექტი იძახებს მას.
            //(ოპერატორების გადატვირთვა, ToString მეთოდის გადატვირთვა, GetHashcode მეთოდის გადატვირთვა)
            //4.აბსტრაქცია = არ უნდა შემეძლოს აბსტრაქტული ტიპების გამოყენება ისევე როგორც კონკრეტული ტიპების.
        }

        static string LogActivity(Client client)
        {
            return $"{client.Identifier} logged in at: {DateTime.Now}";
        }


    }
}
