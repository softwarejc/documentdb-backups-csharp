# Azure DocumentDB and C# (6.0)

Azure DocumentDB, or shortly DocumentDB, is a NoSQL document database service offered by Microsoft. This is a repository with sample code and helpers to it with .NET C# 6.0.

The code contains a very simple console application to make request to Azure DocumentDB.

To make Azure DocumentDB request I wrote a generic class using C# 6.0. This is the interface:

```csharp
public interface IDocumentDbCollection<T> : IDisposable
  where T : Document
  {
    // Creates a new document in database
    Task<T> CreateAsync(T item);
  
    // Gets the document with the specified id
    T Get(string id);
  
    // Gets the list of elements that fulfill the specified predicate
    IEnumerable<T> Where(Expression<Func<T, bool>> predicate = null);
    
    // Gets all documents
    IEnumerable<T> AllDocuments { get; }
  
    // Updates a document
    Task<T> ReplaceAsync(string id, T item);
  
    // Deletes the specified document
    Task DeleteAsync(T item);
}
  ```
An example of how can it be used:

```csharp
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
  foreach (Item item in context.ShoppingList.AllDocuments)
  {
    Console.WriteLine($"\nDelete: {item.Id}");
    Console.WriteLine(item);

    context.ShoppingList.DeleteAsync(item).Wait();
  }
}
 ```
