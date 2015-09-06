# Azure DocumentDB - C# (6.0) library

Azure DocumentDB, or shortly DocumentDB, is a NoSQL document database service offered by Microsoft. 

This library offers a set of classes to work with DocumentDB using .NET C# 6.0.

An example of how can it be used:

```csharp
using (var foodService = new FoodService())
{
    //---
    Console.WriteLine("\n> Create some documents:");
    Console.ReadLine();
    foodService.ShoppingList.CreateDocument(new Item { Name = "Milk", Description = "Skimmed milk" }).Wait();
    foodService.ShoppingList.CreateDocument(new Item { Name = "Milk", Description = "Whole milk" }).Wait();
    foodService.ShoppingList.CreateDocument(new Item { Name = "Water", Description = "Mineral" }).Wait();
    foreach (var item in foodService.ShoppingList.AllDocuments)
    {
        Console.WriteLine($"- {item.Name}- {item.Description}- {item.Id}");
    }

    //---
    Console.WriteLine("\n> Find all items with name = Milk:");
    Console.ReadLine();
    foreach (var item in foodService.ShoppingList.Where(d => d.Name.Equals("Milk")))
    {
        Console.WriteLine($"- {item.Name}- {item.Description}- {item.Id}");
    }

    //---
    Console.WriteLine("\n> Create a backup:");
    Console.ReadLine();
    var backup = foodService.BackupService.CreateBackup("my backup").Result;
    Console.WriteLine("Backup created at" + backup.Timestamp);

    //---
    Console.WriteLine("\n> Delete water:");
    Console.ReadLine();
    foreach (var item in foodService.ShoppingList.Where(d => d.Name.Equals("Water")))
    {
        Console.WriteLine($"Delete: {item.Name} - {item.Id}");
        foodService.ShoppingList.DeleteDocument(item.SelfLink).Wait();
    }

    //---
    Console.WriteLine("\n> Restore backup:");
    Console.ReadLine();
    foodService.BackupService.RestoreBackup(backup);
    Console.WriteLine("Backup restored!");

    //---
    Console.WriteLine("\n> List all the documents:");
    Console.ReadLine();
    foreach (var item in foodService.ShoppingList.AllDocuments)
    {
        Console.WriteLine($"- {item.Name}- {item.Description}- {item.Id}");
    }

    //---
    Console.WriteLine("\n> Delete database:");
    Console.ReadLine();
    foodService.ShoppingList.DatabaseService.DeleteDatabase().Wait();
}
 ```
