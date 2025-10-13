namespace Nine
{
    public class Money
    {
        public decimal Amount { get; set; }
        public string Currecncy { get; set; }

        public Money(decimal amount, string Currency)
        {
            Amount = amount;
            Currecncy = Currency;
        }



        #region Comparision Operators
        public static bool operator ==(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount == b.Amount;
        }

        public static bool operator !=(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount != b.Amount;
        }

        public static bool operator >(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount > b.Amount;
        }
        public static bool operator <(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount < b.Amount;
        }
        public static bool operator >=(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount >= b.Amount;
        }
        public static bool operator <=(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount <= b.Amount;
        }


        #endregion


        #region Unary Operators
        public static Money operator ++(Money a)
        {
            return new Money(a.Amount++, a.Currecncy);
        }
        public static Money operator --(Money a)
        {
            return new Money(a.Amount--, a.Currecncy);
        }
        #endregion


        #region Arithmetical operators
        public static Money operator +(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return new Money(a.Amount + b.Amount, a.Currecncy);
        }

        public static Money operator -(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return new Money(a.Amount - b.Amount, a.Currecncy);
        }

        public static Money operator *(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return new Money(a.Amount * b.Amount, a.Currecncy);
        }

        public static Money operator /(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            EnsureDivideByZeroValidation(b);

            return new Money(a.Amount * b.Amount, a.Currecncy);
        }

        #endregion


        #region Object class methods
        public override bool Equals(object? obj)
        {
            if (obj is Money m)
            {
                EnsureSameCurrency(this, m);
                return Amount == m.Amount;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Amount} {Currecncy}";
        }
        #endregion


        #region Helper Mehtods
        private static void EnsureDivideByZeroValidation(Money b)
        {
            if (b.Amount == 0)
                throw new DivideByZeroException("Can't divide by zero.");
        }
        private static void EnsureSameCurrency(Money a, Money b)
        {
            if (a.Currecncy.ToUpper().Trim() != b.Currecncy.ToUpper().Trim())
            {
                throw new InvalidOperationException("Can't operate two different currencies");
            }
        }
        #endregion

    }
}
