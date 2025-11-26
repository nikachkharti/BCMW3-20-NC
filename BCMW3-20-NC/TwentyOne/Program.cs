namespace TwentyOne
{
    // შაბლონი თუ როგორი უნდა იყოს Handler - ის სტრუქტურა
    public delegate void OrderEventHandler(string orderName, decimal price);

    // PUBLISHER
    public class CoffeeShop
    {
        public event OrderEventHandler OrderPlaced;

        public string Name { get; set; }

        public CoffeeShop(string name)
        {
            Name = name;
        }

        public void PlaceOrder(string orderName, decimal price)
        {
            Console.WriteLine($"\n[{Name}] Processing order: {orderName} (${price})");

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


    // SUBSCRIBER
    public class Kitchen
    {
        public string KitchenName { get; set; }

        public Kitchen(string name)
        {
            KitchenName = name;
        }

        public void OnOrderPlaced(string orderName, decimal price)
        {
            Console.WriteLine($"  [{KitchenName}] Received order! Preparing: {orderName}");
        }
    }


    // SUBSCRIBER
    public class Cashier
    {
        public string CashierName { get; set; }
        private decimal totalSales = 0;

        public Cashier(string name)
        {
            CashierName = name;
        }

        public void OnOrderPlaced(string orderName, decimal price)
        {
            totalSales += price;
            Console.WriteLine($"  [{CashierName}] Payment received: ${price}. Total sales: ${totalSales}");
        }
    }


    public class InventoryManager
    {
        public void OnOrderPlaced(string orderName, decimal price)
        {
            Console.WriteLine($"  [Inventory] Updating stock for: {orderName}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            CoffeeShop shop = new CoffeeShop("Cool Coffee Cafe");

            Kitchen kitchen = new Kitchen("Main Kitchen");
            Cashier cashier = new Cashier("Sarah");
            InventoryManager inventory = new InventoryManager();



            Console.WriteLine("--- SCENARIO 1: No subscribers yet ---");
            shop.PlaceOrder("Espresso", 3.50m);





            Console.WriteLine("\n\n--- SCENARIO 2: Subscribing to events ---");
            Console.WriteLine("Connecting Kitchen to OrderPlaced event...");

            shop.OrderPlaced += kitchen.OnOrderPlaced;

            Console.WriteLine("Connecting Cashier to OrderPlaced event...");
            shop.OrderPlaced += cashier.OnOrderPlaced;

            Console.WriteLine("Connecting Inventory to OrderPlaced event...");
            shop.OrderPlaced += inventory.OnOrderPlaced;





            Console.WriteLine("\n\n--- SCENARIO 3: Placing orders with subscribers ---");
            shop.PlaceOrder("Cappuccino", 4.50m);
            shop.PlaceOrder("Latte", 5.00m);




            Console.WriteLine("\n\n--- SCENARIO 4: Unsubscribing from events ---");
            Console.WriteLine("Disconnecting Kitchen from OrderPlaced event...");

            shop.OrderPlaced -= kitchen.OnOrderPlaced;

            shop.PlaceOrder("Americano", 3.00m);




            //Console.WriteLine("\n\n--- KEY CONCEPTS EXPLAINED ---");
            //Console.WriteLine("1. DELEGATE: A type that defines the signature of event handler methods");
            //Console.WriteLine("2. EVENT: A member of a class that allows other classes to subscribe to notifications");
            //Console.WriteLine("3. PUBLISHER: The class that declares and raises the event (CoffeeShop)");
            //Console.WriteLine("4. SUBSCRIBERS: Classes that respond to the event (Kitchen, Cashier, Inventory)");
            //Console.WriteLine("5. SUBSCRIBE (+=): Connect a method to an event to receive notifications");
            //Console.WriteLine("6. UNSUBSCRIBE (-=): Disconnect a method from an event");
            //Console.WriteLine("7. RAISING: Triggering the event to notify all subscribers");

            //Console.WriteLine("\n\n--- WHY USE EVENTS? ---");
            //Console.WriteLine("• Loose Coupling: CoffeeShop doesn't need to know about Kitchen or Cashier");
            //Console.WriteLine("• Flexibility: Easy to add/remove subscribers without changing CoffeeShop code");
            //Console.WriteLine("• Real-world examples: Button clicks, data changes, timers, network events");

            //Console.WriteLine("\n\nPress any key to exit...");
            //Console.ReadKey();
        }
    }
}
