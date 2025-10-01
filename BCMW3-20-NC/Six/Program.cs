namespace Six
{
    //OOP - ობიექტზე ორიენტირებული პროგრამირება [ენკაფსულაცია, მემკვიდრეობა, პოლიმორფიზმი, აბსტრაქცია]
    //ენკაფსულაცია, გარკვეული ფუნქციონალის წვდომის შეზღუდვა უსაფრთხოების მიზნით.

    public class Student
    {
        private int _age;

        //AUTO PROPERTY
        public string FirstName { get; set; }
        public string LastName { get; set; }


        //FULL PROPERTY
        public int Age
        {
            get { return _age; }
            set
            {
                if (value > 0)
                {
                    _age = value;
                }
            }
        }

    }


    public class Human
    {
        //სავალდებულო
        //მაქსიმუმ ზომა 50
        public string FirstName { get; set; }

        //სავალდებულო
        //მაქსიმუმ ზომა 50
        public string LastName { get; set; }

        //სავალდებულო
        //დადებითი
        public byte Age { get; set; }

        //სავალდებულო
        //ზუსტად 11 ზომაში
        public string PersonalNumber { get; set; }

        //სავალდებულო
        //ზუსტად 9 ზომაში
        public string PhoneNumber { get; set; }

        //სავალდებულო
        //უნდა ერიოს @ .
        //არ უნდა იწყებოდეს @ .
        //არ უნდა მთავრდებოდეს @ .
        //. არ უნდა იჯდეს @ წინ
        public string Email { get; set; }

        public Human(string firstName, string lastName, byte age, string personalNumber, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            PersonalNumber = personalNumber;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public void DisplayInfoInConsole()
        {
            Console.WriteLine($"{FirstName} {LastName} {Age} {PersonalNumber} {PhoneNumber} {Email}");
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {

            Student std1 = new Student();
            std1.FirstName = "Nika";
            std1.LastName = "Chkhartishvili";
            std1.Age = 10;


            Console.WriteLine($"Hello my name is {std1.FirstName} {std1.LastName} I am {std1.Age} years old");



            //Person person1 = new Person();
            //person1.firstName = "Tinatin";
            //person1.lastName = "Khatiashvili";
            //person1._age = 20;

            //person1.Talk();


            //Person person2 = new Person()
            //{
            //    firstName = "Akaki",
            //    lastName = "Zakariadze",
            //    _age = 28
            //};

            //person2.Talk();


            //Person person3 = new Person("Nikoloz", "Ebralidze", 19);
            //person3.Talk();



            //Person person4 = new("Giorgi", "Maisuradze", 21);
            //Person person5 = new("Giorgi", "Maisuradze", 21);

            //person4.Talk();


        }
    }
}
