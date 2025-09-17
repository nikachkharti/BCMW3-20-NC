#region CLASSWORK



//გამაერთიანებელი
//string firstName = "Nika";
//string lastName = "Chkhartishvili";

//string fullName = firstName + " " + lastName;

//Console.WriteLine(fullName);


// არითმეტიკული
//int a = 10;
//int b = 2;

//Console.WriteLine(a + b);
//Console.WriteLine(a - b);
//Console.WriteLine(a * b);
//Console.WriteLine(a / b); // გაყოფა ---> 5
//Console.WriteLine(a % b); // ნაშთიანი გაყოფა ---> 0



// შედარების და მიმართების ოპერატორები
//int x = 5;
//int y = 10;

//Console.WriteLine(x == y); // false
//Console.WriteLine(x != y); // true
//Console.WriteLine(x > y); // false
//Console.WriteLine(x < y); // true
//Console.WriteLine(x >= y); // false
//Console.WriteLine(x <= y); // false


//ლოგიკური ოპერატორები
//bool a = true;
//bool b = false;

//Console.WriteLine(a && b); //false (AND)
//Console.WriteLine(a || b); //true (OR)
//Console.WriteLine(!a); //false (NOT)


//მინიჭების ოპერატორები
//int n = 5;

////n = n + 2;
//n += 2;

////n = n - 2;
//n -= 2;

////n = n * 2;
//n *= 2;

////n = n / 2;
//n /= 2;

////n = n % 2;
//n %= 2;



//Console.WriteLine(n);


//გაზრდა შემცირების ოპერატორები
//int z = 5;

//Console.WriteLine(z += 1);
//Console.WriteLine(z++);
//Console.WriteLine(++z);

//Console.WriteLine(z -= 1);
//Console.WriteLine(z--);
//Console.WriteLine(--z);





//ტერნარი ოპერატორი
//int age = 20;

//string result = (age > 18) ? "Adult" : "Not Adult";
//Console.WriteLine(result);






//NULL - ზე შემოწმების ოპერატორები

//string name = "Nika";

//string displayName = name ?? "Unknown";
//Console.WriteLine(displayName); //Nika


//string name = null;

//string displayName = name ?? "Unknown";
//Console.WriteLine(displayName); //Unknown





//ტიპური ოპერატორები
//object obj = "Hello";
//Console.WriteLine(obj is string); //true

//string castedObject = obj as string;
//Console.WriteLine(castedObject); // Hello





// თუ არ  ვიცით მონაცემი რა ტიპია გამოვიყენოთ GetType()
//int x = 1;
//Console.WriteLine(x.GetType());



//WHILE     2
//DO WHILE  4
//FOR       1
//FOREACH   1



//int i = 1;
//do
//{
//    i++;
//    Console.WriteLine($"{i}.NIKA");
//}
//while (i <= 10);



//int i = 1;
//while (i <= 10)
//{
//    Console.WriteLine("NIKA");
//    i++;
//}



//for (int i = 0; i < 10; i++)
//{
//    Console.WriteLine($"{i}.NIKA");
//}


//List<int> intList = new();
//foreach (var item in intList)
//{

//}

#endregion


#region HOMEWORK

//for while do while foreach


//for (int i = 1; i <= 10; i++)
//{
//    Console.Write(i + " ");
//}


//int sum = 0;
//for (int i = 1; i <= 100; i++)
//{
//    sum += i;
//}

//Console.WriteLine(sum);


//for (int i = 1; i <= 20; i++)
//{
//    if (i % 2 != 0)
//        Console.WriteLine(i);
//}


//for (int i = 1; i <= 9; i++)
//{
//    for (int j = 1; j <= 9; j++)
//    {
//        Console.WriteLine($"{i} * {j} = {i * j}");
//        //Console.WriteLine(i + " " + "*" + j + " " + "=" + " " + i * j);
//        //Console.WriteLine("{0} * {1} = {2}", i, j, i * j);
//    }
//}


//for (int i = 10; i >= 1; i--)
//{
//    Console.WriteLine(i);
//}


//int number = 5;
//int factorial = 1;

//for (int i = 1; i <= number; i++)
//{
//    factorial *= i;
//}

//Console.WriteLine($"Factorial: {factorial}");


//int sum = 0;
//int i = 1;


//while (sum < 50)
//{
//    sum += i;
//    i++;
//}

//Console.WriteLine(sum);


//const string correctPassword = "123";
//string password = string.Empty;

//do
//{
//    Console.Write("Password: ");
//    password = Console.ReadLine();
//}
//while (password != correctPassword);


//string[] names = ["nika", "giorgi", "daviti"];

//foreach (var name in names)
//{
//    Console.WriteLine(name);
//}



//PALINDROME
//int number = 232;
//int reversed = 0;
//int temp = number;

//while (temp > 0)
//{
//    int digit = temp % 10;
//    reversed = reversed * 10 + digit;
//    temp /= 10;
//}

//if (number == reversed)
//    Console.WriteLine("Palindrome");
//else
//    Console.WriteLine("Not Palindrome");


//long number = 100;
//long a = 0;
//long b = 1;

//for (int i = 0; i < number; i++)
//{
//    long t = a + b;
//    a = b;
//    b = t;

//    Console.WriteLine(t);
//}


//for (int i = 0; i < 5; i++)
//{
//    if (i == 3)
//    {
//        // ნიშნავს ციკლის გაჩერებას
//        break;
//    }


//    if (i == 3)
//    {
//        //კონკრეტულ შემთხვევაში ციკლის მუშაობის დროებით შეწყვეტას
//        continue;
//    }

//    Console.WriteLine(i);
//}


// გააკეთეთ კალკულატორი რომელიც შეასრულებს  (+ - * /)
// X  -- ნიშნავდეს აპლიკაციის შეჩერებას.





#endregion