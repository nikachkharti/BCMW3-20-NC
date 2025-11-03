using Algorithms;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Fifteen
{
    public class StudentEquilityComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student? x, Student? y)
        {
            return x.Name.ToLower().Trim() == y.Name.ToLower().Trim();
        }

        public int GetHashCode([DisallowNull] Student obj)
        {
            return obj.Name.Length;
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object? obj) => new StudentEquilityComparer().Equals(obj as Student, this);
        public override int GetHashCode() => new StudentEquilityComparer().GetHashCode(this);
    }


    public class MoneyReference : IEquatable<MoneyReference>
    {
        public decimal Amount { get; set; }
        public string? Currency { get; set; }

        public bool Equals(MoneyReference? other)
        {
            return other.Amount == this.Amount && other.Currency == this.Currency;
        }
    }


    public struct MoneyValue
    {
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
    }


    internal class Program
    {
        static void Main(string[] args)
        {

            //Reference Currency

            //Currency ტიპები არსებობენ მხოლოდ stack memory - ში (struct)
            //Reference ტიპები არსებობენ ორივეგან (class)


            //MoneyValue mv1 = new() { Amount = 100, Currency = "GEL" };
            //MoneyValue mv2 = new() { Amount = 100, Currency = "GEL" };

            //Console.WriteLine(mv1.Equals(mv2));

            MoneyReference mr1 = new() { Amount = 100, Currency = "GEL" };
            MoneyReference mr2 = mr1;

            Console.WriteLine(mr1.Equals(mr2));


            //int[] intAr = [1, 2, 2, 23, 14, 77];

            //HashSet<int> intSet = new() { 11, 21, 42, 23, 23, 23 };
            //Dictionary<string, int> intDic = new()
            //{
            //    {"one",1 },
            //    {"two",2},
            //    {"three",3 }
            //};



            List<int> intList = new() { 1, 2, 2, 23, 77, 77 };
            List<Student> students = new()
            {
                new() { Name = "Giorgi"},
                new() { Name = "Akaki"},
                new() { Name = "Akaki"},
            };

            var result = CustomAlgorithms
                .CustomDistinct(
                students
            //,
            //new StudentEquilityComparer()
            );







        }
    }
}


