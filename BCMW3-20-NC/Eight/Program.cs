using Eight.Guns;

namespace Eight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Player player = new Player("Niko", 5000);
                    Shop shop = new Shop();

                    shop.ShowWeapons();

                    Console.WriteLine("\nEnter weapon to buy:");
                    string weaponName = Console.ReadLine();

                    Weapon weapon = shop.GetWeapon(weaponName);
                    if (weapon == null)
                        throw new InvalidOperationException("Weapon not found in the shop!");

                    player.BuyWeapon(weapon);

                    Console.WriteLine("\nEnter weapon to shoot:");
                    string shootWeapon = Console.ReadLine();
                    player.UseWeapon(shootWeapon);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("\nGame ended.");
                }
            }
        }
    }
}
