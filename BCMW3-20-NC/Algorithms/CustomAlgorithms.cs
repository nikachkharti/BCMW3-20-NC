﻿using Algorithms.Models;
namespace Algorithms
{
    //დელეგატი არის ტიპი რომელსაც შეუძლია მიინიჭოს მეთოდი
    //დელეგატის სტრუქტურა ზუსტად უნდა ემთხვეოდეს
    //იმ მეთოდის მისაღებ და დასაბრუნებელ ტიპებს რომელ მეთოდსაც იგი ინიჭებს

    public delegate bool FindDelegate(int input);
    public delegate Vehicle TransformerDelegate(string input);
    public delegate bool ContainsDelegate(Vehicle input);
    public delegate bool ComparerDelegate(Vehicle input1, Vehicle input2);

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

        /// <summary>
        /// Selection sort
        /// </summary>
        public static Vehicle[] Sort(Vehicle[] vehicles, ComparerDelegate comparerDelegate)
        {
            for (int i = 0; i < vehicles.Length - 1; i++)
            {
                for (int j = i + 1; j < vehicles.Length; j++)
                {
                    if (comparerDelegate(vehicles[j], vehicles[i]))
                    {
                        Vehicle temp = vehicles[j];
                        vehicles[j] = vehicles[i];
                        vehicles[i] = temp;
                    }
                }
            }

            return vehicles;
        }
        public static Vehicle[] FindAll(Vehicle[] vehicles, ContainsDelegate containsDelegate)
        {
            List<Vehicle> result = new();
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (containsDelegate(vehicles[i]))
                {
                    result.Add(vehicles[i]);
                }
            }

            return result.ToArray();
        }
        public static Vehicle[] TransformToVehicles(string[] data, TransformerDelegate transformer)
        {
            Vehicle[] vehicles = new Vehicle[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                vehicles[i] = transformer(data[i]);
            }

            return vehicles;
        }

        public static int Custom_FirstOrDefault(List<int> listCollection, FindDelegate findDelegate)
        {
            for (int i = 0; i < listCollection.Count; i++)
            {
                if (findDelegate(listCollection[i]))
                {
                    return listCollection[i];
                }
            }

            return default;
        }
        public static int Custom_FirstOrDefault(int[] arrayCollection, FindDelegate findDelegate)
        {
            for (int i = 0; i < arrayCollection.Length; i++)
            {
                if (findDelegate(arrayCollection[i]))
                {
                    return arrayCollection[i];
                }
            }

            return default;
        }
        public static int Custom_FirstOrDefault(Dictionary<int, int> dictionaryCollection, FindDelegate findDelegate)
        {
            var keys = new List<int>(dictionaryCollection.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                int key = keys[i];

                if (findDelegate(dictionaryCollection[key]))
                    return dictionaryCollection[key];
            }

            return default;
        }


        //მინდა რომ დამიწეროთ Custom_IndexOf მეთოდი რომელიც იმუშავებს List - ებისთვის და იმუშავებს
        //დელეგატის გამოყენებით

    }
}
