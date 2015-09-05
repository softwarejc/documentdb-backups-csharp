using System;
using System.Threading.Tasks;

using DocumentDB.Framework.Interfaces;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DocumentDB.Framework
{
    public class DocumentDBContext : IDisposable
    {
        private readonly DocumentClient _client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDBContext" /> class.
        /// </summary>
        /// <param name="endPointUrl">The end point URL.</param>
        /// <param name="authorizationKey">The authorization key.</param>
        /// <param name="databaseId">The database Id.</param>
        protected DocumentDBContext(Uri endPointUrl, string authorizationKey, string databaseId)
        {
            _client = new DocumentClient(endPointUrl, authorizationKey);

            DatabaseService = new DatabaseService(_client, databaseId);
        }

        /// <summary>
        ///     Gets the database service.
        /// </summary>
        public IDatabaseService DatabaseService { get; }

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
        protected async Task<IDocumentDBCollection<T>> DocumentDBCollection<T>(string collectionId) where T : Document
        {
            var collection = await DatabaseService.ReadOrCreateCollection(collectionId);
            return new DocumentDBCollection<T>(_client, collection);
        }
    }
}