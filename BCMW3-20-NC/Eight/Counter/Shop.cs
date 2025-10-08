using Eight.Counter.Guns;

namespace Eight.Counter
{
    public class Shop
    {
        private List<Weapon> _weapons = new List<Weapon>();

        public Shop()
        {
            _weapons.Add(new Pistol("Glock", price: 500, ammo: 12));
            _weapons.Add(new Shootgun("Winchseter", price: 2700, ammo: 13));
            _weapons.Add(new Rifle("AK-47", price: 4750, ammo: 30));
        }

        public void ShowWeapons()
        {
            Console.WriteLine("\nAvailable weapons:");
            foreach (var weapon in _weapons)
            {
                Console.WriteLine($"-{weapon.Name} - Price: {weapon.Price} - Ammo: {weapon.Ammo}");
            }
        }

        public Weapon? GetWeapon(string weaponName)
        {
            foreach (var item in _weapons)
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
