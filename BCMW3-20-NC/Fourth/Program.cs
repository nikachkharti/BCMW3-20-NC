namespace Fourth
{
    internal class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("--------------------------------------------");

                try
                {
                    (string name, int age) = GetUserInfo();
                    DisplayUserInfoInConsole(name, age);

                    if (age >= 18)
                    {
                        int[] lotteryNumbers = GetLotteryNumbers();

                        Console.Write("ENTER THE NUMBER : ");
                        int usersVersion = GuessNumber();

                        if (UserWins(lotteryNumbers, usersVersion))
                        {
                            Console.WriteLine("CONGRATULATIONS YOU WIN !!!!!");
                        }
                        else
                        {
                            Console.WriteLine("TRY AGAIN");
                        }
                    }
                    else
                    {
                        Console.WriteLine("AGE MUST BE +=18");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    File.AppendAllText(@"../../../casino-game-logs.txt", $"{ex.Message}-{DateTime.Now}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        static (string name, int age) GetUserInfo() //TUPLE
        {
            Console.Write("Enter username: ");
            string name = Console.ReadLine();

            Console.Write("Enter age: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                throw new FormatException("Format of age must be a numeric value");
            }

            ValidateInput(name, age);

            return (name, age);
        }
        static void DisplayUserInfoInConsole(string name, int age)
        {
            Console.WriteLine($"Hello {name} {age} years old");
        }
        static int[] GetLotteryNumbers()
        {
            Random random = new Random();

            int[] numbers = new int[3];

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(1, 8);
            }

            return numbers;
        }
        static int GuessNumber()
        {
            if (!int.TryParse(Console.ReadLine(), out var number))
            {
                throw new FormatException("Format of number must be a numeric value");
            }

            ValidateInput(number);

            return number;
        }
        static bool UserWins(int[] numbers, int number)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == number)
                {
                    return true;
                }
            }

            return false;
        }


        //გადატვირთული მეთოდი 1
        static void ValidateInput(string nameValue, int ageValue)
        {
            if (string.IsNullOrWhiteSpace(nameValue))
                throw new ArgumentException("Name value is required");

            if (ageValue <= 0)
                throw new ArgumentException("Age value is required and positive");
        }

        //გადატვირთული მეთოდი 2
        static void ValidateInput(int numberValue)
        {
            if (numberValue <= 0)
            {
                throw new ArgumentException("Number value is required and positive");
            }
        }

    }
}
