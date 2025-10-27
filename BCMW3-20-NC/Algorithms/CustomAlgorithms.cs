using Algorithms.Models;
namespace Algorithms
{
    #region DELEGATES 1
    //დელეგატი არის ტიპი რომელსაც შეუძლია მიინიჭოს მეთოდი
    //დელეგატის სტრუქტურა ზუსტად უნდა ემთხვეოდეს
    //იმ მეთოდის მისაღებ და დასაბრუნებელ ტიპებს რომელ მეთოდსაც იგი ინიჭებს


    //public delegate Vehicle TransformerDelegate(string input);
    //public delegate bool ContainsDelegate(Vehicle input);
    //public delegate bool ComparerDelegate(Vehicle input1, Vehicle input2);
    #endregion


    #region DELEGATES 2

    //Action -> არის დელეგატის ტიპი რომელიც ინიჭებს ისეთ ფუნქციას რომელიც აბრუნებს void - ს
    //Func -> არის დელეგატის ტიპი რომელიც ინიჭებს ისეთ ფუნქციას რომელსაც აქვს ჩვენთვის სასურველი დასაბრუნებელი და მისაღები პარამეტრი
    //Predicate -> არის დელეგატის ტიპი რომელიც ინიჭებს ისეთ ფუნქციას რომლის დასაბრუნებელი ტიპი არის კონკრეტულად bool

    #endregion


    /*
•	Reverse აბრუნებს გადაცემული მასივის შეტრიალებულ ვარიანტს
•	Sort აბრუნებს გადაცემული მასივის დალაგებულ ვარიანტს
•	Any აბრუნებს true თუ მასივის რომელიმე ელემენტი ემთხვევა მოსაძებნად  გადაცემულ ელემენტებს
•	All აბრუნებს true თუ მასივის ყველა ელემენტი ემთხვევა მოსაძებნად  გადაცემულ ელემენტებს
•	FirstOrDefault მასივში მოძებნის გადაცემული რიცხვის პირველივე მნიშვნელობას თუ არ მოიძებნა დააბრუნებს default - ს
•	LastOrDefault მასივში მოძებნის გადაცემული რიცხვის ბოლო მნიშვნელობას თუ არ მოიძებნა დააბრუნებს default - ს
•	FindAll მოძებნის და დააბრუნებს მასივის ყველა იმ ელემენტს რომელიც გადაცემულია მოსაძებნად 
•	FindIndex მასივში მოძებნის გადაცემული რიცხვის პირველივე მნიშვნელობის ინდექსს თუ არ მოიძებნა დააბრუნებს -1
•	FindLastIndex მასივში მოძებნის გადაცემული რიცხვის ბოლო მნიშვნელობის ინდექსს თუ არ მოიძებნა დააბრუნებს -1
•	Sum შეკრებს მასივის ყველა ელემენტს.
     
     
     */


    public class CustomAlgorithms
    {
        public static T[] CustomTake<T>(T[] array, int quantity)
        {
            T[] result = new T[quantity];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array[i];
            }

            return result;
        }
        public static TDestination[] CustomSelect<TSource, TDestination>(TSource[] data, Func<TSource, TDestination> selector)
        {
            TDestination[] result = new TDestination[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = selector(data[i]);
            }

            return result;
        }
        public static T[] CustomFindAll<T>(T[] array, Func<T, bool> predicate)
        {
            List<T> result = new();
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                {
                    result.Add(array[i]);
                }
            }

            return result.ToArray();
        }
        public static T CustomFirstOrDefault<T>(List<T> listCollection, Func<T, bool> predicate)
        {
            for (int i = 0; i < listCollection.Count; i++)
            {
                if (predicate(listCollection[i]))
                {
                    return listCollection[i];
                }
            }

            return default;
        }
        public static T CustomFirstOrDefault<T>(T[] arrayCollection, Predicate<T> predicate)
        {
            for (int i = 0; i < arrayCollection.Length; i++)
            {
                if (predicate(arrayCollection[i]))
                {
                    return arrayCollection[i];
                }
            }

            return default;
        }
        public static T[] CustomSort<T>(T[] collection, Func<T, T, bool> comparer)
        {
            for (int i = 0; i < collection.Length - 1; i++)
            {
                for (int j = i + 1; j < collection.Length; j++)
                {
                    if (comparer(collection[j], collection[i]))
                    {
                        T temp = collection[j];
                        collection[j] = collection[i];
                        collection[i] = temp;
                    }
                }
            }

            return collection;
        }
        public static int CustomIndexOf<T>(List<T> collection, Func<T, bool> predicate)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                {
                    return i;
                }
            }

            return -1;
        }


        



        /*
         
        public static int Custom_FirstOrDefault(List<int> stringCollection, FindDelegate predicate)
        {
            for (int i = 0; i < stringCollection.Count; i++)
            {
                if (predicate(stringCollection[i]))
                {
                    return stringCollection[i];
                }
            }

            return default;
        }
        public static int Custom_FirstOrDefault(int[] arrayCollection, FindDelegate predicate)
        {
            for (int i = 0; i < arrayCollection.Length; i++)
            {
                if (predicate(arrayCollection[i]))
                {
                    return arrayCollection[i];
                }
            }

            return default;
        }
        public static int Custom_FirstOrDefault(Dictionary<int, int> dictionaryCollection, FindDelegate predicate)
        {
            var keys = new List<int>(dictionaryCollection.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                int key = keys[i];

                if (predicate(dictionaryCollection[key]))
                    return dictionaryCollection[key];
            }

            return default;
        }

         */

    }
}
