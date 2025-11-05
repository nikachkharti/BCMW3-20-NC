using Algorithms;
using System.Collections;
using System.Threading.Channels;

namespace Sixteen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //LINQ : Linq query | Extension Methods

            List<string> data = new List<string>() { "1", "2", "3", "-1", "-2" };

            //var x = data
            //    .Select(int.Parse)
            //    .OrderBy(x => x)
            //    .Where(x => x < 0)
            //    .FirstOrDefault(x => x % 2 == 0);


            var y =
                (from d in data
                 let x = int.Parse(d)
                 where x < 0
                 orderby x
                 select x
                 ).FirstOrDefault(x => x % 2 == 0);


            //var x = data
            //    .CustomSelect(int.Parse)
            //    .CustomToList()
            //    .CustomOrderBy((x, y) => x < y)
            //    .CustomWhere(x => x < 0)
            //    .CustomFirstOrDefault(x => x % 2 == 0);


            #region OBSOLOTE
            //var dataAsInts = CustomAlgorithms
            //    .CustomSelect(data, int.Parse);

            //var sortedData = CustomAlgorithms
            //    .CustomOrderBy(dataAsInts.ToList(), (x, y) => x < y);

            //var filteredData = CustomAlgorithms
            //    .CustomWhere(sortedData, x => x < 0);

            //var finalResult = CustomAlgorithms
            //    .CustomFirstOrDefault(filteredData, x => x % 2 == 0); 
            #endregion


            //List<int> intList = new() { 11, 12, 7, 33, 21, -88, 351, 12131, 345, 22, 22 };
            //var result = intList.CustomDistinct();




        }
    }
}
