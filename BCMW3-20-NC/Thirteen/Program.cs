using Algorithms;

namespace Thirteen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> intList = new() { 1, 1, 2, -1, 3 };
            List<string> stringList = new() { "Giorgi", "Daviti", "Irakli", "Luka" };


            var result = CustomAlgorithms
                .CustomFirstOrDefault(stringList, x => x.Equals("Irakli"));
        }
    }
}
