using System.Xml.Linq;

namespace Fourth
{
    internal class Program
    {
        static void Main()
        {
            (string name, int age) = GetUserInfo();
            DisplayUserInfoInConsole(name, age);

            if (age >= 18)
            {
                int[] lotteryNumbers = GetLotteryNumbers();

                //1. დაწერეთ მეთოდი რომელიც user - ს შემოაყვანინებს რიცხვს
                //2. დაწერეთ მეთოდი რომელიც მიიღებს user - ისგან შემოყვანილ რიცხვს და მოიფიქრეთ ლოგიკა რა შემთხვევაში იმარჯვებს user  ამ თამაშში.
            }
        }


        static (string name, int age) GetUserInfo() //TUPLE
        {
            string name = Console.ReadLine();
            int age = int.Parse(Console.ReadLine());

            return (name, age);
        }

        static void DisplayUserInfoInConsole(string name, int age)
        {
            Console.WriteLine($"Hello {name} {age} years old");
        }

        static int[] GetLotteryNumbers()
        {
            int[] numbers = new int[3] { 1, 2, 3 };

            return numbers;
        }

    }
}
