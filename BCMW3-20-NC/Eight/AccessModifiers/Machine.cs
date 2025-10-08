namespace Eight.AccessModifiers
{
    public class Machine
    {
        public string MachineName { get; }

        public Machine()
        {
        }

        public Machine(string name)
        {
            MachineName = name;
        }

        protected internal void Start()
        {
            Console.WriteLine("Machine Started");
        }

        private protected void Print()
        {
            Console.WriteLine("PRINTING...");
        }
    }
}
