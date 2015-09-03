using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDb_HelloWorld.Common
{
    /// <summary>
    /// An implementation of IDocumentDbCollection using C# 6.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DocumentDbCollection<T> : IDocumentDbCollection<T>
        where T : Document
    {
        private readonly Database _database;
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

            _database = database;
            _client = client;
            _collection = ReadOrCreateCollection(_database.Id, collectionId).Result;
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

        private async Task<DocumentCollection> ReadOrCreateCollection(string databaseLink, string collectionId)
        {
            var col = _client.CreateDocumentCollectionQuery(_database.SelfLink)
                              .Where(c => c.Id == collectionId)
                              .AsEnumerable()
                              .FirstOrDefault();

            return col ?? (col = await _client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = collectionId }));
        }

        void IDisposable.Dispose()
        {
            _client?.Dispose();
        }
    }
}
