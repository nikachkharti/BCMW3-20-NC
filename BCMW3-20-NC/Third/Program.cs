using System.IO.Pipes;

namespace Third
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string nameNika = "Nika";
            //var nameGiorgi = "Giorgi";
            //var nameSandro = "Sandro";

            //1

            //string[] names = new string[5];
            //names[0] = "Nika";
            //names[1] = "Giorgi";
            //names[2] = "Sandro";
            //names[3] = "Daviti";
            //names[4] = "Elene";
            //names[4] = "Jemali";



            //2
            //string[] names = ["Nika", "Giorgi", "Sandro", "Daviti"];

            //Console.WriteLine(names[0]); //<-- ინდექსი
            //Console.WriteLine(names[1]); //<-- ინდექსი
            //Console.WriteLine(names[2]); //<-- ინდექსი
            //Console.WriteLine(names[3]); //<-- ინდექსი


            //3
            //string[] names = { "Nika", "Giorgi", "Sandro", "Daviti" };

            //for (int i = 0; i < names.Length; i++)
            //{
            //    Console.WriteLine(names[i]);
            //}


            int[] numbers = [10, 20, 340, 30, 40];


            //1. დაწერეთ ლოგიკა, რომელიც მოძებნის მასივში მაქსიმლაურ ელემენტს

            //int max = numbers[0];
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    if (numbers[i] > max)
            //        max = numbers[i];
            //}
            //Console.WriteLine($"MAX: {max}");


            //2. დაწერეთ ლოგიკა, რომელიც მოძებნის მასივში მინიმალურ ელემენტს

            //int min = numbers[0];
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    if (numbers[i] < min)
            //    {
            //        min = numbers[i];
            //    }
            //}

            //Console.WriteLine($"MIN: {min}");


            //3. მასივში ყველა ელემენტი მიუმატეთ ერთმანეთს.

            //int sum = 0;
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    //sum = sum + numbers[i];
            //    sum += numbers[i];
            //}

            //Console.WriteLine($"SUM: {sum}");



        }
    }
}
