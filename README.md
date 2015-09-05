# Azure DocumentDB - C# (6.0) library

Azure DocumentDB, or shortly DocumentDB, is a NoSQL document database service offered by Microsoft. 

This library offers a set of classes to work with DocumentDB using .NET C# 6.0.

An example of how can it be used:

```csharp
using (var foodService = new FoodService())
{
    // Create some items
    foodService.ShoppingList.CreateDocument(new Item { Name = "Milk", Description = "Skimmed milk" }).Wait();
    foodService.ShoppingList.CreateDocument(new Item { Name = "Milk", Description = "Whole milk" }).Wait();
    foodService.ShoppingList.CreateDocument(new Item { Name = "Water", Description = "Mineral" }).Wait();

    // Find all items with name = milk
    foreach (var item in foodService.ShoppingList.Where(d => d.Name.Equals("Milk")))
    {
        Console.WriteLine($"- {item.Name} - {item.Description}");
    }

    // Delete all documents
    foreach (var item in foodService.ShoppingList.AllDocuments)
    {
        Console.WriteLine($"\nDelete: {item.Id}");
        Console.WriteLine(item);

        foodService.ShoppingList.DeleteDocument(item.SelfLink).Wait();
    }

    // Delete database
    foodService.DatabaseService.DeleteDatabase().Wait();
}
 ```
