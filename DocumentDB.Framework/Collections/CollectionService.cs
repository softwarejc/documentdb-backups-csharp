using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DocumentDB.Framework.Database;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Framework.Collections
{
    internal class CollectionService<T> : ICollectionService<T>
        where T : Document
    {
        /// <summary>
        ///     Creates a documentDb repository to perform documents operations against one collection.
        /// </summary>
        /// <param name="client">Document db client</param>
        /// <param name="collection">The collection.</param>
        /// <param name="databaseService">The database service.</param>
        public CollectionService(DocumentClient client, DocumentCollection collection, DatabaseService databaseService)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            Client = client;
            Collection = collection;
            DatabaseService = databaseService;
        }

        /// <summary>
        ///     Gets the documentDB client.
        /// </summary>
        public DocumentClient Client { get; }

        /// <summary>
        ///     Gets the DocumentDB collection
        /// </summary>
        public DocumentCollection Collection { get; }

        /// <summary>
        ///     Gets the database service.
        /// </summary>
        public IDatabaseService DatabaseService { get; }

        /// <summary>
        ///     Gets all documents.
        /// </summary>
        public IEnumerable<T> AllDocuments
        {
            get
            {
                return Where(d => true);
            }
        }

        public Uri CollectionUri => UriFactory.CreateCollectionUri(DatabaseService.Database.Id, Collection.Id);

        /// <summary>
        ///     Creates a document.
        /// </summary>
        public async Task<T> CreateDocument(T item)
        {
            return await Client?.CreateDocumentAsync(Collection.SelfLink, item) as T;
        }

        /// <summary>
        ///     Gets a document.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        public T GetDocumentById(string id)
        {
            return
                Client?.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(d => d.Id == id)
                    .AsEnumerable()
                    .SingleOrDefault();
        }

        public async Task<T> GetDocumentByLink(string documentLink)
        {
            return await Client?.ReadDocumentAsync(documentLink) as T;
        }

        /// <summary>
        ///     Gets the list of elements that fulfill the specified predicate
        /// </summary>
        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Client?.CreateDocumentQuery<T>(Collection.DocumentsLink).Where(predicate).AsEnumerable();
        }

        /// <summary>
        ///     Replaces a document
        /// </summary>
        public async Task<T> ReplaceDocument(string id, T item)
        {
            var doc = GetDocumentById(id);
            if (doc == null)
            {
                throw new InvalidOperationException("Item not found");
            }

            return await Client?.ReplaceDocumentAsync(doc.SelfLink, item) as T;
        }

        /// <summary>
        ///     Deletes the specified document
        /// </summary>
        public async Task DeleteDocument(string documentLink)
        {
            if (documentLink == null)
            {
                throw new ArgumentNullException(nameof(documentLink));
            }

            await Client?.DeleteDocumentAsync(documentLink);
        }
    }
}