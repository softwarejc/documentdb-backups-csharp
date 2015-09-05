using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace DocumentDb_HelloWorld.Common
{
    public interface IDocumentDbCollection<T> 
        where T : Document
    {
        // Creates a new document in database
        Task<T> CreateDocument(T item);

        // Gets the document with the specified id
        T GetDocument(string id);

        // Gets the list of elements that fulfill the specified predicate
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate = null);

        // Gets all documents
        IEnumerable<T> AllDocuments { get; }

        // Updates a document
        Task<T> ReplaceDocument(string id, T item);

        // Deletes the specified document
        Task DeleteDocument(T item);
    }
}