namespace Eight.AccessModifiers
{
    public class Car
    {
        public string Model { get; set; }
        private int enginePower = 200;
        protected int Speed = 220;

        public void StartEngine()
        {
            Console.WriteLine("Engine Startred...");
        }

        private void ShowPower()
        {
            Console.WriteLine($"Power is : {enginePower}");
        }

        internal void Run()
        {
            Console.WriteLine("Engine running.....");
        }
    }

    //public class Lamborgini : Car
    //{
    //    public Lamborgini()
    //    {
    //        Speed = 700;
    //    }
    //}

}
