namespace Six
{
    public class Person // კლასი არის ადგილი სადაც ხდება ობიექტის სტრქუქტურის აღწერა
    {
        public string firstName; // this
        public string lastName; //  this
        public int age; // this

        //კონსტრუქტორი არის ფუნქცია რომელიც ობიექტის აგებაზეა პასუხისმგებელი.
        public Person()
        {
        }
        public Person(string firstName, string lastName, int age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
        }
        public Person(int age)
        {
            this.age = age;
        }

        public Person(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public void Talk()
        {
            Console.WriteLine($"Hello my name is {firstName} {lastName}, I am {age} years old");
        }

    }



    internal class Program
    {
        static void Main(string[] args)
        {
            //ობიექტი ანუ ამ კლასის კონკრეტული გამოვლინება

            Person person1 = new Person();
            person1.firstName = "Tinatin";
            person1.lastName = "Khatiashvili";
            person1.age = 20;

            person1.Talk();


            Person person2 = new Person()
            {
                firstName = "Akaki",
                lastName = "Zakariadze",
                age = 28
            };

            person2.Talk();


            Person person3 = new Person("Nikoloz", "Ebralidze", 19);
            person3.Talk();



            Person person4 = new("Giorgi", "Maisuradze", 21);
            Person person5 = new("Giorgi", "Maisuradze", 21);

            person4.Talk();


        }
    }
}
