namespace Eight.Guns
{
    public abstract class Weapon
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Ammo { get; protected set; }

        public Weapon(string name, decimal price, int ammo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Weapon name can't be empty.");

            if (price <= 0)
                throw new ArgumentException("Weapon price must be greater than zero.");

            if (ammo < 0)
                throw new ArgumentException("Ammo cannot be negative.");

            Name = name;
            Price = price;
            Ammo = ammo;
        }

        public abstract void Shoot();
    }
}
