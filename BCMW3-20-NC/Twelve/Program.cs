using Algorithms;

namespace Twelve
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arrayCollection = [1, 2, 3, 3, 2, 12];

            List<int> listCollection = new();
            listCollection.Add(1);
            listCollection.Add(2);
            listCollection.Add(3);
            listCollection.Add(3);
            listCollection.Add(2);
            listCollection.Add(-12);
            listCollection.Add(-22);

            var result =
                CustomAlgorithms
                .Custom_FirstOrDefault(listCollection, number => number < 0);
        }


        //private static bool FirstNegativeNumber(int elementToFind)
        //{
        //    return elementToFind < 0;
        //}
        //private static bool FirstOddElement(int elementToFind)
        //{
        //    return elementToFind % 2 != 0;
        //}
        //private static bool FirstEvenElement(int elementToFind)
        //{
        //    return elementToFind % 2 == 0;
        //}



    }
}
