using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DocumentDb_HelloWorld.Domain
{
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
}