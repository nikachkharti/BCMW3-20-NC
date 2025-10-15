using Algorithms.Models;
namespace Algorithms
{
    public class CustomAlgorithms
    {
        public static Vehicle[] Take(Vehicle[] vehicles, int quantity)
        {
            if (quantity > vehicles.Length)
                throw new ArgumentException("Quantity can't exceed vehicles length");

            Vehicle[] result = new Vehicle[quantity];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = vehicles[i];
            }

            return result;
        }
        public static Vehicle[] Sort(Vehicle[] vehicles)
        {
            for (int i = 0; i < vehicles.Length - 1; i++)
            {
                for (int j = i + 1; j < vehicles.Length; j++)
                {
                    if (vehicles[j].Combined > vehicles[i].Combined)
                    {
                        Vehicle temp = vehicles[j];
                        vehicles[j] = vehicles[i];
                        vehicles[i] = temp;
                    }
                }
            }

            return vehicles;
        }
        public static Vehicle[] FindAllMercedeses(Vehicle[] vehicles)
        {
            List<Vehicle> mercedeses = new();
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i].Make.Contains("Mercedes", StringComparison.OrdinalIgnoreCase))
                {
                    mercedeses.Add(vehicles[i]);
                }
            }

            return mercedeses.ToArray();
        }
        public static Vehicle[] TransformToVehicles(string[] data)
        {
            Vehicle[] vehicles = new Vehicle[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                vehicles[i] = Vehicle.Parse(data[i]);
            }

            return vehicles;
        }
    }
}
