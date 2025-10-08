using Eight.Counter.Guns;

namespace Eight.Counter
{
    public class Player
    {
        public string Name { get; set; }
        public decimal Money { get; private set; }
        public List<Weapon> Weapons { get; private set; }

        public Player(string name, decimal startinMoney)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            if (startinMoney < 0)
                throw new ArgumentException("Money can't be a negative number");

            Name = name;
            Money = startinMoney;
            Weapons = new List<Weapon>();
        }

        public void BuyWeapon(Weapon weapon)
        {
            if (weapon.Price > Money)
                throw new InvalidOperationException($"{Name} doesn't have enough money to buy {weapon.Name}");

            Money -= weapon.Price;
            Weapons.Add(weapon);

            Console.WriteLine($"{Name} bought {weapon.Name}");
        }

        public void UseWeapon(string weaponName)
        {
            var weapon = GetWeapon(weaponName);

            if (weapon == null)
                throw new InvalidOperationException($"{Name} doesn't own a weapon {weaponName}");

            weapon.Shoot();
        }

        private Weapon? GetWeapon(string weaponName)
        {
            foreach (var item in Weapons)
            {
                if (weaponName.Trim().Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }
    }
}
