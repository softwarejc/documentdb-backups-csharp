using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DocumentDB.Framework.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Framework
{
    internal class DocumentDBCollection<T> : IDocumentDBCollection<T>
        where T : Document
    {
        private readonly DocumentClient _client;

        /// <summary>
        /// Creates a documentDb repository to perform documents operations against one collection.
        /// </summary>
        /// <param name="client">Document db client</param>
        /// <param name="databaseLink">Link to the database that contains the collection</param>
        /// <param name="collectionId">Collection id</param>
        public DocumentDBCollection(DocumentClient client, string databaseLink, string collectionId)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _client = client;

            Collection = ReadOrCreateCollection(databaseLink, collectionId).Result;
        }

        #region IRepository

        /// <summary>
        /// Gets the DocumentDB collection
        /// </summary>
        public DocumentCollection Collection { get; }

        /// <summary>
        /// Gets all documents.
        /// </summary>
        public IEnumerable<T> AllDocuments
        {
            get { return Where(d => true); }
        }

        /// <summary>
        /// Creates a document.
        /// </summary>
        public async Task<T> CreateDocument(T item)
        {
            return await _client?.CreateDocumentAsync(Collection.SelfLink, item) as T;
        }

        /// <summary>
        /// Gets a document.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        public T GetDocument(string id)
        {
            return _client?.CreateDocumentQuery<T>(Collection.DocumentsLink)
                                .Where(d => d.Id == id)
                                .AsEnumerable()
                                .SingleOrDefault();
        }

        /// <summary>
        /// Gets the list of elements that fulfill the specified predicate
        /// </summary>
        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _client?.CreateDocumentQuery<T>(Collection.DocumentsLink)
                        .Where(predicate)
                        .AsEnumerable();
        }

        /// <summary>
        /// Replaces a document
        /// </summary>
        public async Task<T> ReplaceDocument(string id, T item)
        {
            T doc = GetDocument(id);
            if (doc == null) throw new InvalidOperationException("Item not found");

            return await _client?.ReplaceDocumentAsync(doc.SelfLink, item) as T;
        }

        /// <summary>
        /// Deletes the specified document
        /// </summary>
        public async Task DeleteDocument(string documentLink)
        {
            if (documentLink == null) throw new ArgumentNullException(nameof(documentLink));

            await _client?.DeleteDocumentAsync(documentLink);
        }

        #endregion

        private async Task<DocumentCollection> ReadOrCreateCollection(string databaseLink, string collectionId)
        {
            var collection = _client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(c => c.Id.Equals(collectionId))
                              .AsEnumerable()
                              .FirstOrDefault();

            if (collection == null)
            {
                // Collection not found, create it
                collection = await _client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = collectionId });
            }
            return collection;
        }
    }
}
