using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DocumentDb_HelloWorld.Common;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Portable.Common
{
    /// <summary>
    /// An implementation of IDocumentDbCollection using C# 6.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DocumentDbCollection<T> : IDocumentDbCollection<T>
        where T : Document
    {
        private readonly DocumentCollection _collection;
        private readonly DocumentClient _client;

        /// <summary>
        /// Creates a documentDb repository to perform documents operations against one collection.
        /// </summary>
        /// <param name="database">Database that contains the collection</param>
        /// <param name="collectionId">Collection id</param>
        /// <param name="client">Document db client</param>
        public DocumentDbCollection(Database database, string collectionId, DocumentClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _client = client;
            _collection = ReadOrCreateCollection(database, collectionId).Result;
        }

        #region IRepository

        public IEnumerable<T> AllDocuments => Where(d => true);

        public async Task<T> CreateDocument(T item)
        {
            return await _client?.CreateDocumentAsync(_collection.SelfLink, item) as T;
        }

        public T GetDocument(string id)
        {
            return _client?.CreateDocumentQuery<T>(_collection.DocumentsLink)
                                .Where(d => d.Id == id)
                                .AsEnumerable()
                                .SingleOrDefault();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _client?.CreateDocumentQuery<T>(_collection.DocumentsLink)
                        .Where(predicate)
                        .AsEnumerable();
        }

        public async Task<T> ReplaceDocument(string id, T item)
        {
            T doc = GetDocument(id);
            if (doc == null) throw new InvalidOperationException("Item not found");

            return await _client?.ReplaceDocumentAsync(doc.SelfLink, item) as T;
        }

        public async Task DeleteDocument(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            await _client?.DeleteDocumentAsync(item.SelfLink);
        }

        #endregion

        private async Task<DocumentCollection> ReadOrCreateCollection(Database database, string collectionId)
        {
            var collection = _client.CreateDocumentCollectionQuery(database.SelfLink)
                              .Where(c => c.Id.Equals(collectionId))
                              .AsEnumerable()
                              .FirstOrDefault();

            if (collection == null)
            {
                // Collection not found, create it
                collection = await _client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionId });
            }
            return collection;
        }
    }
}
