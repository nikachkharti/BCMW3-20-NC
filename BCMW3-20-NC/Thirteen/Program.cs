using Algorithms;

namespace Thirteen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<int> intList = new() { 1, 1, 2, -1, 3 };
            //List<string> stringList = new() { "Giorgi", "Daviti", "Irakli", "Luka" };

            //var result = CustomAlgorithms.CustomSort(intList.ToArray(), (x, y) => x % 2 == 0);

            TestClassForGeneric<int> firstPerson = new();
            firstPerson.Identifier = 1;
            firstPerson.Name = "Nika";

            TestClassForGeneric<int> secondPerson = new();
            secondPerson.Identifier = 2;
            secondPerson.Name = "Daviti";


            TestClassForGeneric<Guid> firstAnimal = new();
            firstAnimal.Identifier = Guid.NewGuid();
            firstAnimal.Name = "Jeka";

            TestClassForGeneric<Guid> secondAnimal = new();
            secondAnimal.Identifier = Guid.NewGuid();
            secondAnimal.Name = "Garrfield";


        }
    }
}
