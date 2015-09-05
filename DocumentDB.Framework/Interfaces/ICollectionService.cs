using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;

namespace DocumentDB.Framework.Interfaces
{
    public interface ICollectionService<T>
        where T : Document
    {
        /// <summary>
        ///     Gets the DocumentDB collection.
        /// </summary>
        DocumentCollection Collection { get; }

        /// <summary>
        ///     Gets all documents.
        /// </summary>
        IEnumerable<T> AllDocuments { get; }

        /// <summary>
        ///     Creates a new document in database.
        /// </summary>
        Task<T> CreateDocument(T document);

        /// <summary>
        ///     Gets the document with the specified id.
        /// </summary>
        T GetDocument(string id);

        /// <summary>
        ///     Gets the list of elements that fulfill the specified predicate.
        /// </summary>
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        ///     Updates a document.
        /// </summary>
        Task<T> ReplaceDocument(string id, T document);

        /// <summary>
        ///     Deletes a document by link.
        /// </summary>
        Task DeleteDocument(string documentLink);
    }
}