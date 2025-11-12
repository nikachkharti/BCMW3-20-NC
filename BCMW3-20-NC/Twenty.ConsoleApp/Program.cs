namespace Twenty.ConsoleApp
{
    internal class Program
    {
        #region Race Condition
        public static int counter = 0;
        public static object lockObject = new();

        //Monitor (lock არის მონიტორის syntax sugar)
        //public static SemaphoreSlim semaphore = new(1, 1); // დამოუკიდებლად გადახედეთ
        //Mutex დამოუკიდებლად გადახედეთ
        //ReaderWriterLock // დამოუკიდებლად გადახედეთ
        //ReaderWriterLockSlim // დამოუკიდებლად გადახედეთ 
        #endregion

        static void Main(string[] args)
        {


        }

        #region Race Condition
        /*
            Thread t1 = new(IncrementCounter);
            Thread t2 = new(IncrementCounter);
            Thread t3 = new(IncrementCounter);

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Console.WriteLine($"Expected: {3 * 100000}");
            Console.WriteLine($"Actual: {counter}");

        */

        private static void IncrementCounter()
        {
            for (int i = 0; i < 100000; i++)
            {
                lock (lockObject)
                {
                    counter++;
                }
            }
        }
        #endregion

        #region Array Example
        /*
            int[] ar = [1, 2, 3, 4, 5, 6, 7, 8];
            ProcessArray(ar);

            Console.WriteLine(string.Join(", ", ar));
        */

        private static void ProcessArray(int[] ar)
        {
            int mid = ar.Length / 2;

            Thread t1 = new(() => Pow(ar, 0, mid));
            Thread t2 = new(() => Pow(ar, mid, ar.Length));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }
        private static void Pow(int[] ar, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Thread.Sleep(1000);
                ar[i] *= i;
            }
        }
        #endregion

        #region Multithreading Example
        /*
            Thread currentThread = Thread.CurrentThread;
            Console.WriteLine($"{currentThread.ManagedThreadId} Started");

            Thread t1 = new(() => IncrmentTimer());
            Thread t2 = new(() => DecrementTimer());

            t1.Priority = ThreadPriority.Highest;
            t2.Priority = ThreadPriority.Lowest;

            t1.Start();
            t2.Start();

            Console.WriteLine($"{currentThread.ManagedThreadId} Finished");
        */

        private static void DecrementTimer()
        {
            Thread currentThread = Thread.CurrentThread;
            Console.WriteLine($"{currentThread.ManagedThreadId} Started");

            for (int i = 10; i > 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            }

            Console.WriteLine($"{currentThread.ManagedThreadId} Finished");
        }
        private static void IncrmentTimer()
        {
            Thread currentThread = Thread.CurrentThread;
            Console.WriteLine($"{currentThread.ManagedThreadId} Started");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            }

            Console.WriteLine($"{currentThread.ManagedThreadId} Finished");
        }
        #endregion
    }
}
