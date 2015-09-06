using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DocumentDB.Framework.Database;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DocumentDB.Framework.Collections
{
    public interface ICollectionService<T>
        where T : Document
    {
        /// <summary>
        ///     Gets the client.
        /// </summary>
        DocumentClient Client { get; }

        /// <summary>
        ///     Gets the DocumentDB collection.
        /// </summary>
        DocumentCollection Collection { get; }

        /// <summary>
        ///     Gets the database service.
        /// </summary>
        IDatabaseService DatabaseService { get; }

        /// <summary>
        ///     Gets all documents.
        /// </summary>
        IEnumerable<T> AllDocuments { get; }

        Uri CollectionUri { get; }

        /// <summary>
        ///     Creates a new document in database.
        /// </summary>
        Task<T> CreateDocument(T document);

        /// <summary>
        ///     Gets the document with the specified id.
        /// </summary>
        T GetDocumentById(string id);

        /// <summary>
        ///     Gets the document with the specified link.
        /// </summary>
        Task<T> GetDocumentByLink(string documentLink);

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