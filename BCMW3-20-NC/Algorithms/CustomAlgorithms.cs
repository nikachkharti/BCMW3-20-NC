using Algorithms.Models;
using System.Collections;
using System.Reflection.Emit;
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


    public static class CustomAlgorithms
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
        public static IEnumerable<TDestination> CustomSelect<TSource, TDestination>(this IEnumerable<TSource> src, Func<TSource, TDestination> selector)
        {
            foreach (var item in src)
            {
                yield return selector(item);
            }
        }
        public static IEnumerable<T> CustomWhere<T>(this IEnumerable<T> src, Func<T, bool> predicate)
        {
            foreach (var item in src)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static T CustomFirstOrDefault<T>(this IEnumerable<T> src, Predicate<T> predicate)
        {
            foreach (var item in src)
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }
        public static IEnumerable<T> CustomForeach<T>(this IEnumerable<T> source)
        {
            IEnumerator<T> enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        public static IList<T> CustomOrderBy<T>(this IList<T> collection, Func<T, T, bool> comparer)
        {
            for (int i = 0; i < collection.Count - 1; i++)
            {
                for (int j = i + 1; j < collection.Count; j++)
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
        public static int CustomIndexOf<T>(this IEnumerable<T> src, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (var item in src)
            {
                if (predicate(item))
                    return i;
                i++;
            }

            return -1;
        }
        public static IEnumerable<T> CustomDistinct<T>(this IEnumerable<T> src, IEqualityComparer<T> comparer = default)
        {
            HashSet<T> result = new HashSet<T>(comparer);

            foreach (var item in src)
            {
                if (result.Add(item))
                    yield return item;
            }
        }
        public static List<T> CustomToList<T>(this IEnumerable<T> src)
        {
            return new List<T>(src);
        }

    }
}
