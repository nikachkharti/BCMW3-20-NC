using Algorithms;
using System.Collections;

namespace Fifteen
{

    internal class Program
    {
        static void Main(string[] args)
        {
            int[] intAr = [1, 2, 2, 23, 14, 77];
            List<int> intList = new() { 1, 2, 2, 23, 14, 77 };
            HashSet<int> intSet = new() { 11, 21, 42, 23, 23, 23 };
            Dictionary<string, int> intDic = new()
            {
                {"one",1 },
                {"two",2},
                {"three",3 }
            };


            //var all = CustomAlgorithms.CustomFindAll(intDic, x => x.Value % 2 == 0);

            var result = CustomAlgorithms.CustomSelect(
                new List<string>() { "1", "2", "3" }, 
                int.Parse
            );



        }
    }
}


