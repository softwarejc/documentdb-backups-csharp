# Azure DocumentDB, Hello world from C# (6.0)
This is a repository with sample code and helpers to use Microsoft Azure DocumentDb from C#.

The code contains a very simple console application to make request to Azure DocumentDB.

To make Azure DocumentDB request I wrote a generic class using C# 6.0. This is the interface:

```csharp
  public interface IDocumentDbCollection<T> : IDisposable
        where T : Document
    {
        Task<T> CreateAsync(T item);

        T Get(Expression<Func<T, bool>> predicate);

        T Get(string id);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate = null);

        IEnumerable<T> Documents { get; }

        Task<T> ReplaceAsync(string id, T item);

        Task DeleteAsync(string id);

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
                    foreach (Item item in context.ShoppingList.Documents)
                    {
                        Console.WriteLine($"\nDelete: {item.Id}");
                        Console.WriteLine(item);

                        context.ShoppingList.DeleteAsync(item).Wait();
                    }
                }
 ```
