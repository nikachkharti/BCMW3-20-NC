using XMLConsoleApp.Models;
using XMLConsoleApp.Service;

/*
    XDocument — represents an XML document.
    XElement — represents an XML node/element.
    XAttribute — represents an attribute.     
 */

var dataFile = Path.Combine(@"../../../Data", "library.xml");
Directory.CreateDirectory(Path.GetDirectoryName(dataFile)!);

var xmlService = new XmlLibraryService(dataFile);


await DemoAsync();

async Task DemoAsync()
{
    Console.WriteLine("--- LINQ to XML: Demo ---\n");

    // Ensure initial data exists
    await xmlService.EnsureSampleDataAsync();


    // Read all books
    var books = await xmlService.GetAllBooksAsync();
    Console.WriteLine("Books in library (initial):");
    foreach (var b in books)
        Console.WriteLine(b);


    // Add a book
    var newBook = new Book { Id = xmlService.NextId(), Title = "Clean Code", Author = "Robert C. Martin", Year = 2008 };
    await xmlService.AddBookAsync(newBook);
    Console.WriteLine($"\nAdded: {newBook}");


    // Update a book
    var toUpdate = (await xmlService.GetAllBooksAsync()).FirstOrDefault();
    if (toUpdate != null)
    {
        toUpdate.Title += " (Updated)";
        await xmlService.UpdateBookAsync(toUpdate);
        Console.WriteLine($"\nUpdated book id={toUpdate.Id}");
    }


    // Remove by id
    var removeId = newBook.Id;
    await xmlService.RemoveBookAsync(removeId);
    Console.WriteLine($"\nRemoved book id={removeId}");


    // Query example
    var recent = (await xmlService.GetAllBooksAsync()).Where(b => b.Year >= 2015);
    Console.WriteLine("\nBooks from 2015+:");
    foreach (var b in recent)
        Console.WriteLine(b);


    Console.WriteLine("\nDemo finished. Press any key to exit.");
    Console.ReadKey();
}