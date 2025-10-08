using Eight.AccessModifiers;
using Eight.Counter;
using Eight.Counter.Guns;

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
                    Player playerJohn = new Player("John", 5000);

                    Shop shop = new();
                    shop.ShowWeapons();

                    Console.Write("Enter weapon name to buy: ");
                    string weaponName = Console.ReadLine();

                    Weapon weapon = shop.GetWeapon(weaponName);

                    if (weapon == null)
                        throw new InvalidOperationException("Weapon not found in the shop");

                    playerJohn.BuyWeapon(weapon);

                    Console.Write("Enter weapon name to shoot: ");
                    string shootWeapon = Console.ReadLine();
                    playerJohn.UseWeapon(shootWeapon);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine($"\nGame ended.");
                }
            }
        }

    }
}
