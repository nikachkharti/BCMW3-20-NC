using Eight.AccessModifiers;

namespace Eight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //PUBLIC -- ღიაა ყველასთვის
            //PRIVATE -- დახურულია ყველასთვის Native კლასის გარდა
            //PROTECTED -- რომელსაც ვიყენებთ მხოლოდ Natvive კლასის ან მემკვიდრის შიგნით
            //INTERNAL -- რომელსაც ვიყენებთ ყველგან მიმდინარე პროექტის - დონეზე.
            //PROTECTED INTERNAL -- ღიაა მემკვიდრეებთან და თან სხვა პროექტის შიგნითაც
            //PRIVATE PROTECTED -- ღიაა Native კლასში და მემკვიდრე კლასებშიც.

            Machine m = new Machine("T5");

            Car car = new Car();
            car.Run();
            car.Model = "BMW";
            car.StartEngine();
        }

    }
}
