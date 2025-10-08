namespace Eight.Counter.Guns
{
    public abstract class Weapon
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Ammo { get; protected set; }

        public Weapon(string name, decimal price, int ammo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Weapon name is required");

            if (price <= 0)
                throw new ArgumentException("Weapon price must be positive");

            Name = name;
            Price = price;
            Ammo = ammo;
        }

        public abstract void Shoot();
    }
}
