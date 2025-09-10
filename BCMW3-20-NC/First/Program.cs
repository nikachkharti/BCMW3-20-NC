namespace First
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // "Hello From C#" -- string
            // 'C' -- char

            //byte sbyte short ushort int uint long ulong
            //float double decimal
            //bool (true | false)


            //კონვერტაცია (ცხადი (Explicit), არაცხადი (Implicit))

            //int x = 258;
            //byte y = (byte)x;

            //int x = Convert.ToInt32("12");
            //int x = byte.Parse("15");

            //byte firstNumber = 12;
            //int secondNumber = Convert.ToInt32(firstNumber);









            //ააწყვეთ მარტივი სარეგისტრაციო ფორმა სადაც მომხმარებელს სთხოვთ რომ კონსოლიდან შემოიყვანის:
            //ყველა შემოყვანილი ინფორმაცია უნდა შეინახოთ შესაბამის მონაცემთა ტიპში
            //სახელი
            //გვარი
            //ასაკი
            //პირადი ნომერი

            // საბოლოო შედეგი დაბეჭდეთ კონსოლში ამ შაბლონის შესაბამისად
            //Hello my name is {firstName} {lastName} I am {age} years old. My Personal number is {personalNumber}




            //Console.Write("FirstName: ");
            //string firstName = Console.ReadLine();

            //Console.Write("LastName: ");
            //string lastName = Console.ReadLine();

            //Console.Write("Age: ");
            //byte age = byte.Parse(Console.ReadLine());


            //Console.Write("Personal number: ");
            //string personalNumber = Console.ReadLine();



            //Console.WriteLine("---------------------------------------------");

            //Console.WriteLine($"Hello my name is {firstName} {lastName} I am {age} years old. My personal number is {personalNumber}");


            int numberToCheck = 32;

            if (numberToCheck > 50)
            {
                Console.WriteLine("MORE");
            }
            else if (numberToCheck == 50)
            {
                Console.WriteLine("EQUAL");
            }
            else if (numberToCheck == 32)
            {
                numberToCheck = 13;
                Console.WriteLine(numberToCheck);
            }
            else if (numberToCheck == 51)
            {
                Console.WriteLine("51");
            }
            else
            {
                Console.WriteLine("LESS");
            }

        }
    }
}
