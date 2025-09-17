namespace First
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region CLASSWORK

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


            //int numberToCheck = 32;

            //if (numberToCheck > 50)
            //{
            //    Console.WriteLine("MORE");
            //}
            //else if (numberToCheck == 50)
            //{
            //    Console.WriteLine("EQUAL");
            //}
            //else if (numberToCheck == 32)
            //{
            //    numberToCheck = 13;
            //    Console.WriteLine(numberToCheck);
            //}
            //else if (numberToCheck == 51)
            //{
            //    Console.WriteLine("51");
            //}
            //else
            //{
            //    Console.WriteLine("LESS");
            //}

            #endregion


            #region HOMEWORK

            // = ერთი ტოლობა ნიშნავს მინიჭებას
            // == ნიშნავს ორი რაღაცის შედარებას

            //Console.Write("Enter number: ");
            //int number = int.Parse(Console.ReadLine());


            //switch (number)
            //{
            //    case 1:
            //        Console.WriteLine("Number is 1");
            //        break;
            //    case 2:
            //        Console.WriteLine("Number is 2");
            //        break;
            //    case 3:
            //        Console.WriteLine("Number is 3");
            //        break;
            //    default:
            //        Console.WriteLine("Other number");
            //        break;
            //}


            //if (number == 1)
            //{
            //    Console.WriteLine("Number is 1");
            //}
            //else if (number == 2)
            //{
            //    Console.WriteLine("Number is 2");
            //}
            //else if (number == 3)
            //{
            //    Console.WriteLine("Number is 3");
            //}
            //else
            //{
            //    Console.WriteLine("Other number");
            //}



            //Console.Write("Enter age: ");
            //byte age = byte.Parse(Console.ReadLine());


            //ტერნარული ოპერატორი ? :
            //string isAdult = age >= 18 ? "Adult" : "No adult";

            //if (age >= 18)
            //{
            //    isAdult = "Adult";
            //}
            //else
            //{
            //    isAdult = "No adult";
            //}




            //const string validAdmin = "admin";
            //const string validPassword = "123";


            //Console.WriteLine("UserName");
            //string userName = Console.ReadLine();


            //Console.WriteLine("Password");
            //string password = Console.ReadLine();


            //if (userName == validAdmin && password == validPassword)
            //{
            //    Console.WriteLine("Successful login");
            //}
            //else
            //{
            //    Console.WriteLine("Invalid user");
            //}

            //string result =
            //    userName == validAdmin && password == validPassword
            //    ? "Successful login"
            //    : "Invalid user";

            //Console.WriteLine(result);



            //int mark = 87;
            //switch (mark)
            //{
            //    case int m when m >= 90:
            //        Console.WriteLine("A+");
            //        break;
            //    case int m when m >= 80:
            //        Console.WriteLine("A");
            //        break;
            //    case int m when m >= 70:
            //        Console.WriteLine("B");
            //        break;
            //    default:
            //        Console.WriteLine("C or below");
            //        break;
            //}


            //decimal amount = 5000;
            //decimal discount = amount > 1000 ? amount * 0.1m : amount * 0.05m;

            //Console.WriteLine($"Discount price is : {discount} GEL");


            //int day = 3;

            //switch (day)
            //{
            //    case 1:
            //        Console.WriteLine("Monday");
            //        break;
            //    case 2:
            //        Console.WriteLine("Tuesday");
            //        break;
            //    case 3:
            //        Console.WriteLine("Wednesday");
            //        break;
            //    case 4:
            //        Console.WriteLine("Thursday");
            //        break;
            //    case 5:
            //        Console.WriteLine("Friday");
            //        break;
            //    case 6:
            //        Console.WriteLine("Satuarday");
            //        break;
            //    case 7:
            //        Console.WriteLine("Sunday");
            //        break;
            //    default:
            //        Console.WriteLine("Invalid number");
            //        break;
            //}



            //string signal = "Green";

            //switch (signal.ToLower())
            //{
            //    case "red":
            //        Console.WriteLine("Stop!");
            //        break;
            //    case "yellow":
            //        Console.WriteLine("Get Ready!");
            //        break;
            //    case "green":
            //        Console.WriteLine("Go!");
            //        break;
            //    default:
            //        Console.WriteLine("Invalid signal");
            //        break;
            //}


            //decimal balance = 500;
            //decimal withdrawal = 600;

            //if (withdrawal <= 0)
            //    Console.WriteLine("Invalid withdrawal amount");
            //else if (withdrawal > balance)
            //    Console.WriteLine("Insufficient balance");
            //else
            //{
            //    balance -= withdrawal;
            //    Console.WriteLine($"Withdrawal successful! Remaining balance: {balance}");
            //}


            //int score = 45;
            //string result = (score >= 50) ? "Pass" : "Fail";
            //Console.WriteLine($"Student result: {result}");


            //int age = 17;

            //if (age < 0)
            //    Console.WriteLine("Invalid age");
            //else
            //    Console.WriteLine((age >= 18) ? "Eligible to vote" : "Not eligible to vote");



            //string role = "Manager";
            //decimal salary = 3000;
            //decimal bonus;

            //switch (role.ToLower())
            //{
            //    case "manager":
            //        bonus = (salary > 2500) ? salary * 0.15m : salary * 0.1m;
            //        break;
            //    case "developer":
            //        bonus = (salary > 2000) ? salary * 0.12m : salary * 0.08m;
            //        break;
            //    default:
            //        bonus = salary * 0.05m;
            //        break;
            //}

            //Console.WriteLine($"Role: {role}, Bonus: {bonus}");



            #endregion

        }
    }
}
