namespace TwentyOne.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string data = await
                File
                .ReadAllTextAsync(@"C:\Users\User\Desktop\New folder (2)\BCMW3-20-NC\BCMW3-20-NC\Twenty.ConsoleApp\test.txt");

            //alskndlaknsd --- data
            //dlasdasdljkasnd  ---- data
            //alsdnlakjds
        }

    }
}
