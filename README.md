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
