namespace TwentyoneConsoleApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public override string ToString() => $"[{Id}] [{Title}] by [{Id}] {Author} ({Year})";
    }
}
