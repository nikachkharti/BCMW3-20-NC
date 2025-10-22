using Algorithms;
using Algorithms.Models;

namespace Ten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines(@"../../../vehicles.csv");

            var vehicles = CustomAlgorithms
                .TransformToVehicles(data, Vehicle.Parse);

            var opels =
                CustomAlgorithms
                .FindAll(vehicles, v => v.Make.Contains("Opel", StringComparison.OrdinalIgnoreCase));

            var sortedVehicles = CustomAlgorithms.Sort(vehicles, (Vehicle v1, Vehicle v2) => v1.Combined < v2.Combined);
            var topTenEcoVehicles = CustomAlgorithms.Take(sortedVehicles, 60);


        }


        //private static bool CompareHighway(Vehicle v1, Vehicle v2)
        //{
        //    return v1.Highway < v2.Highway;
        //}
        //private static bool CompareNonEconomicCars(Vehicle v1, Vehicle v2)
        //{
        //    return v1.Combined < v2.Combined;
        //}
        //private static bool CompareEconmicCars(Vehicle v1, Vehicle v2)
        //{
        //    return v1.Combined > v2.Combined;
        //}




        //private static bool OpelExists(Vehicle vehicle)
        //{
        //    return vehicle.Make
        //        .Contains("Opel", StringComparison.OrdinalIgnoreCase);
        //}
        //private static bool MercedesExists(Vehicle vehicle)
        //{
        //    return
        //        vehicle.Make
        //        .Contains("Mercedes", StringComparison.OrdinalIgnoreCase);
        //}
        //private static bool BmwExists(Vehicle vehicle)
        //{
        //    return
        //        vehicle.Make
        //        .Contains("BMW", StringComparison.OrdinalIgnoreCase);
        //}


    }
}
