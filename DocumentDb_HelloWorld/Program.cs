using System;

using DocumentDb_HelloWorld.Domain;

namespace DocumentDb_HelloWorld
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                using (var context = new MyFoodContext())
                {
                    Console.WriteLine("Create some documents:");
                    Console.ReadLine();

                    // Create some items
                    context.ShoppingList.CreateDocument(new Item { Name = "Milk", Description = "Skimmed milk" }).Wait();
                    context.ShoppingList.CreateDocument(new Item { Name = "Milk", Description = "Whole milk" }).Wait();
                    context.ShoppingList.CreateDocument(new Item { Name = "Water", Description = "Mineral" }).Wait();

                    // Find all items with name = milk
                    Console.WriteLine("> Find 'Milk':");
                    foreach (var item in context.ShoppingList.Where(d => d.Name.Equals("Milk")))
                    {
                        Console.WriteLine($"- {item.Name} - {item.Description}");
                    }

                    // Delete all documents
                    Console.WriteLine("\n> Delete all:");
                    Console.ReadLine();

                    foreach (var item in context.ShoppingList.AllDocuments)
                    {
                        Console.WriteLine($"\nDelete: {item.Id}");
                        Console.WriteLine(item);

                        context.ShoppingList.DeleteDocument(item.SelfLink).Wait();
                    }

                    // Delete database
                    context.DatabaseService.DeleteDatabase().Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                Console.WriteLine("\nPress <ENTER> to close the app.");
                Console.ReadLine();
            }
        }
    }
}