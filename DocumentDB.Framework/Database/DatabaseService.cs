using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Framework.Database
{
    internal class DatabaseService : IDatabaseService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseService" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="databaseId">The database identifier.</param>
        public DatabaseService(DocumentClient client, string databaseId)
        {
            Client = client;
            Database = ReadOrCreateDatabase(Client, databaseId).Result;
        }

        /// <summary>
        ///     Gets the documentDB client.
        /// </summary>
        protected DocumentClient Client { get; }

        /// <summary>
        ///     Gets the database associated to this service.
        /// </summary>
        public Microsoft.Azure.Documents.Database Database { get; }


        /// <summary>
        ///     Deletes the service database.
        /// </summary>
        public async Task DeleteDatabase()
        {
            await Client.DeleteDatabaseAsync(Database.SelfLink);
        }

        /// <summary>
        ///     Reads the or create collection.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        /// <returns></returns>
        public async Task<DocumentCollection> ReadOrCreateCollection(string collectionId)
        {
            var collection =
                Client.CreateDocumentCollectionQuery(Database.SelfLink)
                    .Where(c => c.Id.Equals(collectionId))
                    .AsEnumerable()
                    .FirstOrDefault();

            if (collection == null)
            {
                // Collection not found, create it
                collection =
                    await
                    Client.CreateDocumentCollectionAsync(
                        Database.SelfLink,
                        new DocumentCollection { Id = collectionId });
            }
            return collection;
        }

        /// <summary>
        ///     Deletes a collection.
        /// </summary>
        public async Task DeleteCollection(DocumentCollection collection)
        {
            await Client.DeleteDocumentCollectionAsync(collection.SelfLink);
        }

        /// <summary>
        ///     Creates a new user
        /// </summary>
        public async Task<User> ReadOrCreateUser(string userId)
        {
            var user = Client.CreateUserQuery(Database.UsersLink).AsEnumerable().FirstOrDefault(u => u.Id == userId);

            // If user does not exists, create it
            if (user == null)
            {
                await Client.CreateUserAsync(DatabaseUri, new User { Id = userId });
            }

            return user;
        }

        /// <summary>
        ///     Creates a permission with an access token for the specified user and the specified collection
        /// </summary>
        public async Task<Permission> CreateUserPermission(
            User user,
            DocumentCollection collection,
            PermissionMode permission)
        {
            var permissionId = permission + collection.Id;

            // The permission may already exists on database, try to find it
            var collectionPermission =
                Client.CreatePermissionQuery(
                    "/dbs/" + Database.ResourceId + "/users/" + user.ResourceId + "/permissions")
                    .AsEnumerable()
                    .FirstOrDefault(u => u.Id == permissionId);

            // If permission not found, create a new one
            if (collectionPermission == null)
            {
                collectionPermission = new Permission
                                           {
                                               PermissionMode = permission,
                                               ResourceLink = collection.SelfLink,
                                               Id = permissionId
                                           };
            }

            return await Client.CreatePermissionAsync(user.SelfLink, collectionPermission);
        }

        /// <summary>
        /// Gets the database URI.
        /// </summary>
        public Uri DatabaseUri => UriFactory.CreateDatabaseUri(Database.Id);

        /// <summary>
        ///     Reads or create a database.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="databaseId">The database identifier.</param>
        public static async Task<Microsoft.Azure.Documents.Database> ReadOrCreateDatabase(
            DocumentClient client,
            string databaseId)
        {
            // Get the database
            var db = client.CreateDatabaseQuery().Where(d => d.Id == databaseId).AsEnumerable().FirstOrDefault();

            // Return the database or create it if not exists yet
            return db ?? await client.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = databaseId });
        }
    }
}