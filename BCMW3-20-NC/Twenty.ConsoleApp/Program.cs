
using System.Diagnostics.Tracing;

namespace Twenty.ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {

        }


        #region Race Condition

        static int Counter = 0;
        static object _lock = new();

        /*

        Console.WriteLine("Starting race condition demo...\n");

        Thread t1 = new Thread(IncrementCounter);
        Thread t2 = new Thread(IncrementCounter);
        Thread t3 = new Thread(IncrementCounter);

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine($"Expected Counter: {3 * 100000}");
        Console.WriteLine($"Actual Counter:   {Counter}");
        Console.WriteLine("\nRun multiple times — you'll see different results due to race conditions!");

         */

        static void IncrementCounter()
        {
            Lock incrementLocker = new();

            for (int i = 0; i < 100000; i++)
            {
                lock (_lock)
                {
                    Counter++; // Race condition occurs here!
                }
            }
        }
        #endregion

        #region Divide and Conquer

        /*
            int[] ar = [1, 2, 3, 4, 5, 6, 7, 8];
            ProcessLargeArray(ar);

            Console.WriteLine(string.Join(", ", ar));         
        */
        private static void ProcessLargeArray(int[] largeArray)
        {
            int mid = largeArray.Length / 2;

            Thread t1 = new(() => ProcessPart(largeArray, 0, mid));
            Thread t2 = new(() => ProcessPart(largeArray, mid, largeArray.Length));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Processing Complete");
        }
        private static void ProcessPart(int[] largeArray, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Thread.Sleep(1000);
                largeArray[i] *= 2;
            }
        }

        #endregion

        #region Multithreading Example
        private static void DecrementTime()
        {
            for (int i = 10 - 1; i >= 0; i--)
            {
                var threadName = Thread.CurrentThread.ManagedThreadId;

                Thread.Sleep(1000);
                i--;
                Console.WriteLine($"[{threadName}] {i}");
            }
        }
        private static void IncrementTimer()
        {
            for (int i = 0; i < 10; i++)
            {
                var threadName = Thread.CurrentThread.ManagedThreadId;

                Thread.Sleep(1000);
                i++;
                Console.WriteLine($"[{threadName}] {i}");
            }
        }
        #endregion

    }
}
