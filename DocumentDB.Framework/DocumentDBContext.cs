using System;

using DocumentDB.Framework.Interfaces;
using DocumentDB.Framework.Services;

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
            UsersService = new UsersService(_client, DatabaseService.Database);
        }

        /// <summary>
        ///     Gets the users service.
        /// </summary>
        public IUsersService UsersService { get; }

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
        protected IDocumentDBCollection<T> Collection<T>(string collectionId) where T : Document
        {
            return new DocumentDBCollection<T>(_client, DatabaseService.Database.SelfLink, collectionId);
        }
    }
}