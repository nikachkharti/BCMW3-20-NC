using Algorithms;
using Algorithms.Models;

namespace Thirteen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> intList = new() { 1, 1, 2, -1, 3 };
            List<string> stringList = new() { "Giorgi", "Daviti", "Irakli", "Luka" };
            List<Vehicle> cars = new();


            var result = CustomAlgorithms.Custom_IndexOf(intList, x => x == -1);

        }
    }
}
