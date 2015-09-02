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
        private readonly string _databaseId;
        private readonly string _collectionId;

        private Database _database;
        private DocumentCollection _documentCollection;

        /// <summary>
        /// Creates a documentDb repository to perform documents operations against one collection.
        /// </summary>
        /// <param name="databaseId">Database id</param>
        /// <param name="collectionId">Collection id</param>
        public DocumentDbCollection(string databaseId, string collectionId)
        {
            _databaseId = databaseId;
            _collectionId = collectionId;

            // Create document db client
            var endpoint = new Uri(ConfigurationManager.AppSettings["endPointUrl"]);
            var authKey = ConfigurationManager.AppSettings["authorizationKey"];
            Client = new DocumentClient(endpoint, authKey);
        }

        #region IRepository

        public IEnumerable<T> AllDocuments => Where(d => true);

        public async Task<T> CreateDocument(T item)
        {
            return await Client?.CreateDocumentAsync(Collection.SelfLink, item) as T;
        }

        public T GetDocument(string id)
        {
            return Client?.CreateDocumentQuery<T>(Collection.DocumentsLink)
                                .Where(d => d.Id == id)
                                .AsEnumerable()
                                .SingleOrDefault();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Client?.CreateDocumentQuery<T>(Collection.DocumentsLink)
                        .Where(predicate)
                        .AsEnumerable();
        }

        public async Task<T> ReplaceDocument(string id, T item)
        {
            T doc = GetDocument(id);
            if (doc == null) throw new InvalidOperationException("Item not found");

            return await Client?.ReplaceDocumentAsync(doc.SelfLink, item) as T;
        }

        public async Task DeleteDocument(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            await Client?.DeleteDocumentAsync(item.SelfLink);
        }

        #endregion

        #region privates

        private Database Database => _database ?? (_database = ReadOrCreateDatabase(_databaseId).Result);

        private DocumentCollection Collection => _documentCollection ?? (_documentCollection = ReadOrCreateCollection(_databaseId, _collectionId).Result);

        private DocumentClient Client { get; }

        private async Task<DocumentCollection> ReadOrCreateCollection(string databaseLink, string collectionId)
        {
            var col = Client.CreateDocumentCollectionQuery(Database.SelfLink)
                              .Where(c => c.Id == collectionId)
                              .AsEnumerable()
                              .FirstOrDefault();

            return col ?? (col = await Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = collectionId }));
        }

        private async Task<Database> ReadOrCreateDatabase(string databaseId)
        {
            var db = Client.CreateDatabaseQuery()
                            .Where(d => d.Id == databaseId)
                            .AsEnumerable()
                            .FirstOrDefault();

            return db ?? (db = await Client.CreateDatabaseAsync(new Database { Id = databaseId }));
        }

        void IDisposable.Dispose()
        {
            Client?.Dispose();
        }

        #endregion
    }
}
