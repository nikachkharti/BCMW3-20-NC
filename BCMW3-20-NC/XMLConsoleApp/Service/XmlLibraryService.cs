using System.Xml.Linq;
using XMLConsoleApp.Models;

namespace XMLConsoleApp.Service
{
    public class XmlLibraryService
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _fileLock = new(1, 1);
        private readonly XName RootName = "Library";
        private static readonly XName BookName = "Book";

        public XmlLibraryService(string filePath)
        {
            _filePath = filePath;
        }

        //1. ფაილის ჩატვირთვა ან შექმნა.
        private XDocument LoadOrCreate()
        {
            if (!File.Exists(_filePath))
            {
                var root = new XElement(RootName);
                var newDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                newDoc.Save(_filePath);

                return newDoc;
            }

            return XDocument.Load(_filePath);
        }

        // 2. აბრუნებს მომდევნო Id - ს ავტო ინკრემენტი.
        public int NextId()
        {
            XDocument doc = LoadOrCreate();
            var max = doc
                .Descendants(BookName) //ამოაქვს Book ელემენტის ყველა შვილი
                .Select(x => (int?)x.Attribute("id") ?? 0)
                .DefaultIfEmpty(0)
                .Max();
            return max + 1;
        }

        private void Save(XDocument doc)
        {
            // Save to temp file then move (atomic-ish)
            var temp = _filePath + ".tmp";
            doc.Save(temp);
            File.Copy(temp, _filePath, overwrite: true);
            File.Delete(temp);
        }

        public async Task EnsureSampleDataAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                var doc = LoadOrCreate();
                if (!doc.Descendants(BookName).Any())
                {
                    doc.Root!.Add(
                    new XElement(BookName,
                    new XAttribute("id", 1),
                    new XElement("Title", "C# in Depth"),
                    new XElement("Author", "Jon Skeet"),
                    new XElement("Year", 2019)
                    ),
                    new XElement(BookName,
                    new XAttribute("id", 2),
                    new XElement("Title", "Pro ASP.NET Core"),
                    new XElement("Author", "Adam Freeman"),
                    new XElement("Year", 2024)
                    )
                    );
                    Save(doc);
                }
            }
            finally { _fileLock.Release(); }
        }
        public async Task<List<Book>> GetAllBooksAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                var doc = LoadOrCreate();
                return doc.Descendants(BookName)
                .Select(x => ToDto(x))
                .OrderBy(b => b.Id)
                .ToList();
            }
            finally { _fileLock.Release(); }
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            await _fileLock.WaitAsync();
            try
            {
                var doc = LoadOrCreate();
                var el = doc.Descendants(BookName)
                .FirstOrDefault(x => (int?)x.Attribute("id") == id);
                if (el == null) return null;
                return ToDto(el);
            }
            finally { _fileLock.Release(); }
        }
        public async Task AddBookAsync(Book book)
        {
            await _fileLock.WaitAsync();
            try
            {
                var doc = LoadOrCreate();
                var el = FromDto(book);
                doc.Root!.Add(el);
                Save(doc);
            }
            finally { _fileLock.Release(); }
        }
        public async Task<bool> UpdateBookAsync(Book book)
        {
            await _fileLock.WaitAsync();
            try
            {
                var doc = LoadOrCreate();
                var el = doc.Descendants(BookName)
                .FirstOrDefault(x => (int?)x.Attribute("id") == book.Id);
                if (el == null) return false;


                el.SetElementValue("Title", book.Title);
                el.SetElementValue("Author", book.Author);
                el.SetElementValue("Year", book.Year);
                Save(doc);
                return true;
            }
            finally { _fileLock.Release(); }
        }
        public async Task<bool> RemoveBookAsync(int id)
        {
            await _fileLock.WaitAsync();
            try
            {
                var doc = LoadOrCreate();
                var el = doc.Descendants(BookName)
                .FirstOrDefault(x => (int?)x.Attribute("id") == id);
                if (el == null) return false;
                el.Remove();
                Save(doc);
                return true;
            }
            finally { _fileLock.Release(); }
        }

        #region Converters
        private static Book ToDto(XElement el)
        {
            return new Book
            {
                Id = (int)el.Attribute("id"),
                Title = (string)el.Element("Title") ?? string.Empty,
                Author = (string)el.Element("Author") ?? string.Empty,
                Year = (int?)el.Element("Year") ?? 0
            };
        }

        private static XElement FromDto(Book b)
        {
            return new XElement(BookName,
            new XAttribute("id", b.Id),
            new XElement("Title", b.Title),
            new XElement("Author", b.Author),
            new XElement("Year", b.Year)
            );
        }
        #endregion
    }
}
