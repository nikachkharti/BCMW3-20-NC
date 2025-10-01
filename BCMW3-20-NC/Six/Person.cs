namespace Six
{
    public class Person
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
}
