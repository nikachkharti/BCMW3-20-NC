using Eight.Guns;

namespace Eight
{
    public class Player
    {
        public string Name { get; set; }
        public decimal Money { get; private set; }
        public List<Weapon> Inventory { get; private set; }

        public Player(string name, decimal startingMoney)
        {
            if (startingMoney < 0)
                throw new ArgumentException("Money cannot be negative.");

            Name = name;
            Money = startingMoney;
            Inventory = new List<Weapon>();
        }

        public void BuyWeapon(Weapon weapon)
        {
            if (weapon.Price > Money)
                throw new InvalidOperationException($"{Name} does not have enough money to buy {weapon.Name}!");

            Money -= weapon.Price;
            Inventory.Add(weapon);
            Console.WriteLine($"{Name} bought {weapon.Name} for ${weapon.Price}. Remaining money: ${Money}");
        }

        public void UseWeapon(string weaponName)
        {
            var weapon = Inventory.FirstOrDefault(w => w.Name == weaponName);
            if (weapon == null)
                throw new InvalidOperationException($"{Name} does not own {weaponName}!");

            weapon.Shoot();
        }
    }
}
