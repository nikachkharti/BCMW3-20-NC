using Eight.Guns;
using Eight.Guns.Children;

namespace Eight
{
    public class Shop
    {
        private Weapon[] _weapons = new Weapon[3];

        public Shop()
        {
            _weapons[0] = new Pistol("Glock", price: 500, ammo: 12);
            _weapons[1] = new Rifle("AK-47", price: 2700, ammo: 30);
            _weapons[2] = new Shootgun("Mossberg 500", price: 4750, ammo: 13);
        }

        public void ShowWeapons()
        {
            Console.WriteLine("\nAvailable weapons:");
            foreach (var w in _weapons)
            {
                Console.WriteLine($"- {w.Name} (${w.Price}) | Ammo: {w.Ammo}");
            }
        }

        public Weapon? GetWeapon(string name)
        {
            return _weapons.FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
