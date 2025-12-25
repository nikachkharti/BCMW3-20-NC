using Microsoft.EntityFrameworkCore;

namespace EFCoreTutorial
{
    // add-migration InitialCreate -- მიგრაციის ინსტრუქციის შექმნა
    // update-database -- მიგრაციების გამოყენება ბაზაზე

    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using var db = new ApplicationDbContext();
                db.Database.EnsureCreated(); // თუ ბაზა არ არსებობს, ამ ბაზას შემიქმნის.

                while (true)
                {
                    Console.WriteLine("\n1. Create 2. Read 3. Update 4. Delete 5. Exit");

                    var choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.Write("Enter Name: ");
                        var name = Console.ReadLine();

                        await db.Users.AddAsync(new Entities.User() { Name = name });
                        await db.SaveChangesAsync(); //COMMIT

                        Console.WriteLine("User added");
                    }
                    else if (choice == "2")
                    {
                        var users = await db.Users.ToListAsync();
                        if (users.Any())
                        {
                            users.ForEach(u => Console.WriteLine($"{u.Id} --- {u.Name}"));
                        }
                        else
                        {
                            Console.WriteLine("No users found.");
                        }
                    }
                    else if (choice == "3")
                    {
                        Console.Write("Enter User Id to update: ");
                        if (int.TryParse(Console.ReadLine(), out int userId))
                        {
                            var user = await db.Users.FindAsync(userId);
                            if (user != null)
                            {
                                Console.Write("Enter new Name: ");
                                user.Name = Console.ReadLine();
                                await db.SaveChangesAsync();
                                Console.WriteLine("User updated");
                            }
                            else
                            {
                                Console.WriteLine("User not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Id.");
                        }
                    }
                    else if (choice == "4")
                    {
                        Console.Write("Enter User Id to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int userId))
                        {
                            var user = await db.Users.FindAsync(userId);
                            if (user != null)
                            {
                                db.Users.Remove(user);
                                await db.SaveChangesAsync();
                                Console.WriteLine("User deleted");
                            }
                            else
                            {
                                Console.WriteLine("User not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Id.");
                        }
                    }
                    else if (choice == "5")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL: {ex.Message}");
            }
        }
    }
}
