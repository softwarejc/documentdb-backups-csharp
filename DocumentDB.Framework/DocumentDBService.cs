using System;
using System.Threading.Tasks;

using DocumentDB.Framework.Backups;
using DocumentDB.Framework.Collections;
using DocumentDB.Framework.Database;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DocumentDB.Framework
{
    public class DocumentDBService : IDisposable
    {
        private readonly DocumentClient _client;
        private readonly string _databaseId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDBService" /> class.
        /// </summary>
        /// <param name="endPointUrl">The end point URL.</param>
        /// <param name="authorizationKey">The authorization key.</param>
        /// <param name="databaseId">The database Id.</param>
        protected DocumentDBService(string endPointUrl, string authorizationKey, string databaseId)
        {
            _databaseId = databaseId;

            _client = new DocumentClient(new Uri(endPointUrl), authorizationKey);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _client.Dispose();
        }

        /// <summary>
        ///     Reads the or create collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionId">The collection identifier.</param>
        /// <returns></returns>
        protected async Task<ICollectionService<T>> CreateCollectionService<T>(string collectionId) where T : Document
        {
            var databaseService = new DatabaseService(_client, _databaseId);
            var collection = await databaseService.ReadOrCreateCollection(collectionId);

            return new CollectionService<T>(_client, collection, databaseService);
        }

        /// <summary>
        ///     Creates a service to make backups of the specified collection.
        /// </summary>
        protected IBackupService CreateCollectionBackupService<T>(ICollectionService<T> collectionSource) where T : Document
        {
            // Get or Create Backup collection
            var backupCollectionService = CreateCollectionService<Backup>(collectionSource.Collection.Id + "_backup").Result;
            return new BackupService((ICollectionService<Document>)collectionSource, backupCollectionService);
        }
    }
}