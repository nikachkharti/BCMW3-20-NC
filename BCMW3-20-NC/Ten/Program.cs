using Algorithms;

namespace Ten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines(@"../../../vehicles.csv");

            var vehicles = CustomAlgorithms.TransformToVehicles(data);
            //var mercedeses = CustomAlgorithms.FindAllMercedeses(vehicles);
            var sortedVehicles = CustomAlgorithms.Sort(vehicles);
            var topTenEcoVehicles = CustomAlgorithms.Take(sortedVehicles, 60);
        }

    }
}
