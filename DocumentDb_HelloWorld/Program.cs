using DocumentDb_HelloWorld.Domain;
using System;

namespace DocumentDb_HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var context = new MyFoodContext())
                {
                    // Create some items
                    context.ShoppingList.CreateAsync(new Item { Name = "Milk", Description = "Skimmed milk" }).Wait();
                    context.ShoppingList.CreateAsync(new Item { Name = "Milk", Description = "Whole milk" }).Wait();
                    context.ShoppingList.CreateAsync(new Item { Name = "Water", Description = "Mineral" }).Wait();

                    // Find all items with name = milk
                    Console.WriteLine("> Find 'Milk':");
                    foreach (Item item in context.ShoppingList.Where(d => d.Name.Equals("Milk")))
                    {
                        Console.WriteLine($"- {item.Name} - {item.Description}");
                    }

                    // Delete all
                    Console.WriteLine("\n> Delete all:");
                    foreach (Item item in context.ShoppingList.Documents)
                    {
                        Console.WriteLine($"\nDelete: {item.Id}");
                        Console.WriteLine(item);

                        context.ShoppingList.DeleteAsync(item).Wait();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                Console.WriteLine("\n\n\nPress <ENTER> to close the app.");
                Console.ReadLine();
            }
        }
    }
}
