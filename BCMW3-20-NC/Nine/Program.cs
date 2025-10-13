using System.Collections.Generic;
using System.Reflection.Emit;

namespace Nine
{
    #region Interfaces
    public interface IAnimal
    {
        public string Name { get; set; }
        void MakeSound();
        void Test()
        {
            Console.WriteLine("TEST");
        }
    }
    public class Dog : IAnimal
    {
        public string Name { get; set; }

        public void MakeSound()
        {
            Console.WriteLine("Bark");
        }

        private void Helper()
        {
        }
    }
    public class Cat : IAnimal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public void MakeSound()
        {
            Console.WriteLine("Meow");
        }

        public void Dance()
        {
            Console.WriteLine("Dancing..");
        }
    }


    public interface IReadable
    {
        void Read();
        void Test2(int x);
    }
    public interface IWritable
    {
        void Write();
        void Test()
        {
            Console.WriteLine("Nika");
        }
        void Test2(string x);
    }
    public interface IDocumentType
    {
        public string Type { get; set; }
        void Test2(bool x);

        void Read();
    }
    public class Document : IReadable, IWritable, IDocumentType
    {
        public string Type { get; set; }

        void IReadable.Read()
        {
            if (Type.Equals("png", StringComparison.OrdinalIgnoreCase)
                || Type.Equals("jpg", StringComparison.OrdinalIgnoreCase)
                || Type.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Reading Image File");
            }
            else if (Type.Equals("mp3", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Reading Audio File");
            }
            else if (Type.Equals("mp4", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Reading Video File");
            }
        }

        void IDocumentType.Read()
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            if (Type.Equals("png", StringComparison.OrdinalIgnoreCase)
                || Type.Equals("jpg", StringComparison.OrdinalIgnoreCase)
                || Type.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Writing Image File");
            }
            else if (Type.Equals("mp3", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Writing Audio File");
            }
            else if (Type.Equals("mp4", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Writing Video File");
            }
        }

        public void Test2(int x)
        {
            throw new NotImplementedException();
        }

        public void Test2(string x)
        {
            throw new NotImplementedException();
        }

        public void Test2(bool x)
        {
            throw new NotImplementedException();
        }
    } 
    #endregion


    internal class Program
    {
        static void Main(string[] args)
        {
            #region Interfaces
            //Document d = new();
            //d.Type = "Mp3";

            //((IWritable)d).Test();
            //((IDocumentType)d).Read();
            //((IReadable)d).Read(); 
            #endregion


        }
    }
}
