using TwentyoneConsoleApp.Models;
using TwentyoneConsoleApp.Service;

/*
    ---XDocument---
    <Library>
        ---XElement---
        <Book Id = 1 ---XAttribute--- > 
            <Author>Nika CHkharitshivli</Author>
            <Name>Nika CHkharitshivli</Name>
        </Book>
    </Library>    





    1.XmlSerializer
    2.Xml.Linq ---

 */



var dataFile = Path.Combine(@"../../../Data", "library.xml");
Directory.CreateDirectory(Path.GetDirectoryName(dataFile));

var xmlService = new XmlLibraryService(dataFile);

Console.WriteLine("----START----");

await xmlService.EnsureSampleDataAsync();

//Read ALlLines
var books = await xmlService.GetAllBooksAsync();
Console.WriteLine("Books in library");
books.ForEach(book => Console.WriteLine(book));


var newBook = new Book { Id = xmlService.NextId(), Title = "Clean Code", Author = "Robert C. Martin", Year = 2008 };
await xmlService.AddBookAsync(newBook);
Console.WriteLine($"\nAdded: {newBook}");


var toUpdate = (await xmlService.GetAllBooksAsync()).FirstOrDefault();

if (toUpdate != null)
{
    toUpdate.Title = "UPDATED";
    await xmlService.UpdateBookAsync(toUpdate);
    Console.WriteLine($"\nUpdated book id: {newBook.Id}");
}

var removeId = newBook.Id;
await xmlService.RemoveBookAsync(removeId);
Console.WriteLine($"\nRemoved book id: {newBook.Id}");



Console.ReadKey();