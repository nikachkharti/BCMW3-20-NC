namespace TwentyOne
{
    // შაბლონი თუ როგორი უნდა იყოს Handler- ის სტრუქტურა
    public delegate void OrderEventHandler(string orderName, decimal price);

    public class CoffeeShop //PUBLISHER
    {
        public event OrderEventHandler OrderPlaced;

        public string Name { get; set; }
        public CoffeeShop(string name)
        {
            Name = name;
        }

        //1. კაფეში შემიკვეთეს ყავა
        public void PlaceOrder(string orderName, decimal price)
        {
            Console.WriteLine($"\n [{Name}] Processing order: {orderName} ${price}");

            if (OrderPlaced != null)
            {
                Console.WriteLine($"[{Name}] Notifying all subscribers...");
                OrderPlaced(orderName, price);
            }
            else
            {
                Console.WriteLine($"[{Name}] No subscribers to notify.");
            }
        }
    }


    public class Kitchen //SUBSCRIBER
    {
        public string KitchneName { get; set; }

        public Kitchen(string name)
        {
            KitchneName = name;
        }

        //3. შეკვეთილს ყავას ადუღებენ სამზარეულოში
        public void OnOrderPlaced(string orderName, decimal price) // HANDLER
        {
            Console.WriteLine($"[{KitchneName}] Recived order! Preparing: {orderName}");
        }

    }


    public class Cahsier //SUBSCRIBER
    {
        public string CashierName { get; set; }
        private decimal totalSales = 0;

        public Cahsier(string name)
        {
            CashierName = name;
        }

        //2. შეკვეთა მიიღო მოლარემ
        public void OnOrderPlaced(string orderName, decimal price) // HANDLER
        {
            totalSales += price;
            Console.WriteLine($"[{CashierName}] Payment received! ${price}. Total sales: ${totalSales}");
        }

    }


    public class ShopManager //SUBSCRIBER
    {
        //4. შეკვეთა მიიღო მენეჯერმა
        public void OnOrderPlaced(string orderName, decimal price) // HANDLER
        {
            Console.WriteLine($"Inventory Manager updaing stock for: {orderName}");
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            CoffeeShop shop = new("Mcdonalds");
            Kitchen kitchen = new("Main Kitchen");
            Cahsier cashier = new("John");
            ShopManager manager = new ShopManager();


            Console.WriteLine("--- SECNARIO:1 No subscribers yet ---");
            shop.PlaceOrder("Espresso", 3.50m);



            Console.WriteLine("--- SECNARIO:2 Subscribing to events ---");

            Console.WriteLine($"Connection cashier to OrderPlaced event...");
            shop.OrderPlaced += cashier.OnOrderPlaced;

            Console.WriteLine($"Connection Kitchen to OrderPlaced event...");
            shop.OrderPlaced += kitchen.OnOrderPlaced;

            Console.WriteLine($"Connection manager to OrderPlaced event...");
            shop.OrderPlaced += manager.OnOrderPlaced;



            Console.WriteLine("--- SECNARIO:3 Unsubscribing to events ---");
            shop.OrderPlaced -= manager.OnOrderPlaced;


            //დელეგატი: ტიპი რომელიც განსაზღვრავს event - ის ბუნებას და handler მეთოდების ბუნებას
            //Event: კლასი რომელიც აძლევს სხვა კლასებს იმის საშუალებას რომ მასზე გააკეთონ subscribe
            //PUBLISHER: ტიპი რომელიც აღძრავს მოვლენას
            //SUBSCRIBER: ტიპი რომელიც ამუშავებს მოვლენას

        }
    }
}
