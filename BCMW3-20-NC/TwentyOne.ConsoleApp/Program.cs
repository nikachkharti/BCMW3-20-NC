using System.Text.Json;

namespace TwentyOne.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }


        #region 4 ContinueWith, თან მოვძებნოთ რომელი კლანი არის პირველი API Response - ში.
        //როგორც მივხვდით Task.Result ბლოკავს მთავარ ნაკადს სანამ ტასკი არ დასრულდება.
        //ტასკის დასრულების შემდეგ. ContinueWith - ის შიგნით შეგვიძლია თამამად მივიღოთ ტასკის შედეგი Result - ით
        //რადგან ბლოკირება ხდება მხოლოდ ContinueWith - ის შიგნით და არა მთავარ ნაკადში.


        /*

            using var httpClient = new HttpClient();
            {
                var task = httpClient.GetStringAsync("https://dattebayo-api.onrender.com/clans");
                task.ContinueWith(t =>
                {
                    var result = t.Result; // აქ შეგვიძლია მივიღოთ ტასკის შედეგი Result - ით რადგან ბლოკირება ხდება მხოლოდ ContinueWith - ის შიგნით და არა მთავარ ნაკადში.

                    //Console.WriteLine(result);

                    var doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;
                    JsonElement clans = root.GetProperty("clans");
                    JsonElement firstClan = clans[0];

                    Console.WriteLine($"First clan's name is {firstClan.GetProperty("name")}");

                });
                task.Wait(); // ეს არის საჭირო რათა მთავარი ნაკადი არ დასრულდეს სანამ ტასკი არ დასრულდება და ContinueWith - ის შიგნით კოდი არ შესრულდება.

                Console.WriteLine("This is the end of the program"); // ეს შესრულდება მყისიერად, შეგვიძლია დავრწმუნდეთ რომ მთავარი ნაკადი არ ბლოკირდება.
            }

           */


        #endregion

        #region 3.2 HTTP Client Example
        //using var httpClient = new HttpClient();
        //var task = httpClient.GetStringAsync("https://dattebayo-api.onrender.com/clans");
        //var result = task.Result;
        #endregion

        #region 3.1. Wait & WaitAll & WaitAny. Result - ით შედეგის აღება გულისხმობს Wait - ის გამოყენებას. ამიტომ Result ბლოკავს სხვა Thread - რაც მთალიანად უკარგავს ასინქრონულობას აზრს. ამიტომაც გვაქვს მეთოდი ContinueWith.

        //var sum = 0; // ეს ცვლადი ცხადდება მთავარ ნაკადში, ამიტომ მისი ცვლილების სანახავად საჭიროა იმ ნაკადების დალოდება რომლებიც მას ცვლიან. რაც მუშაობს ზუსტად ისევე როგორც Join

        //Task task = Task.Run(() =>
        //{
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        Task.Delay(100);
        //        sum += i;
        //    }
        //});

        // ზუსტად იგივე რაც Join() სხვა ყველა ნაკადი იბლოკება სანამ ეს ტასკი არ დასრულდება
        //task.Wait();

        // ზუსტად იგივე რაც Join() არგუმენტად იღებს Task - ების მასივს და ყველა ტასკის დასრულებამდე ბლოკავს მთავარ ნაკადს
        //Task.WaitAll(task);

        // ზუსტად იგივე რაც Join() არგუმენტად იღებს Task - ების მასივს და რომელიმე ტასკის დასრულებამდე ბლოკავს მთავარ ნაკადს
        //Task.WaitAny(task);

        //Console.WriteLine($"The result is: {sum}");

        #endregion

        #region 2. Task Result

        //Task<int> x = Task.Run(() => Sum(10, 20));
        //int result = x.Result;

        private static int Sum(int arg1, int arg2) => arg1 + arg2;

        #endregion

        #region 1. Task Syntax. Uses thread pool by default

        //Thread thread1 = new(() => Dowork("Hello From Thread"));
        //thread1.Start();

        //Task task1 = new(() => Dowork("Hello From Task"));
        //task1.Start();

        //Task task1Result = Task.Run(() => Dowork("Hello From Task"));
        //task1Result.Wait();

        //Task.WaitAll(task1Result, task2Result);

        private static void Dowork(string text) => Console.WriteLine(text);

        #endregion

    }
}
