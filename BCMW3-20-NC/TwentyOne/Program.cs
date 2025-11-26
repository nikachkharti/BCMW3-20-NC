namespace TwentyOne
{
    public class OrderEventArgs : EventArgs
    {
        public string OrderName { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderTime { get; set; }

        public OrderEventArgs(string orderName, decimal price, DateTime orderTime)
        {
            OrderName = orderName;
            Price = price;
            OrderTime = orderTime;
        }
    }


    // შაბლონი თუ როგორი უნდა იყოს Handler- ის სტრუქტურა
    public delegate void OrderEventHandler(object sender, OrderEventArgs e);

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
                OrderEventArgs args = new(orderName, price, DateTime.Now);
                OrderPlaced(this, args);
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
        public void OnOrderPlaced(object sender, OrderEventArgs e) // HANDLER
        {
            CoffeeShop shop = sender as CoffeeShop;

            Console.WriteLine($"[{KitchneName}] Recived order! Preparing: {shop.Name}");
            Console.WriteLine($"[{KitchneName}] Preparing: {shop.Name} at {e.OrderTime:HH:mm:ss}");
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
        public void OnOrderPlaced(object sender, OrderEventArgs e) // HANDLER
        {
            totalSales += e.Price;
            Console.WriteLine($"[{CashierName}] Payment received! ${e.OrderName}. Total sales: ${totalSales}");
            Console.WriteLine($"[{CashierName}] Preparing: {e.OrderName} processed at {e.OrderTime:HH:mm:ss}");

        }

    }


    public class ShopManager //SUBSCRIBER
    {
        //4. შეკვეთა მიიღო მენეჯერმა
        public void OnOrderPlaced(object sender, OrderEventArgs e) // HANDLER
        {
            Console.WriteLine($"Inventory Manager updaing stock for: {e.OrderName}");
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
