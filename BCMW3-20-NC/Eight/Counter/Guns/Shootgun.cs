namespace Eight.Counter.Guns
{
    public class Shootgun : Weapon
    {
        public Shootgun(string name, decimal price, int ammo) : base(name, price, ammo)
        {
        }

        public override void Shoot()
        {
            if (Ammo < 3)
                throw new InvalidOperationException($"{Name} has no bullets left");

            Ammo -= 3;
            Console.WriteLine($"{Name} fires multipe bullets! (Ammo left: {Ammo})");
        }
    }
}
