namespace Eight.Counter.Guns
{
    public class Pistol : Weapon
    {
        public Pistol(string name, decimal price, int ammo) : base(name, price, ammo)
        {
        }

        public override void Shoot()
        {
            if (Ammo <= 0)
                throw new InvalidOperationException($"{Name} has no bullets left");

            Ammo--;
            Console.WriteLine($"{Name} fires a single bullet! (Ammo left: {Ammo})");
        }
    }
}
