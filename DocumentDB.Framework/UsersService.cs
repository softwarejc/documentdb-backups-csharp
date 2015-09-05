using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Framework
{
    public class UsersService : IUsersService
    {
        private readonly DocumentClient _client;
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="database">The database.</param>
        public UsersService(DocumentClient client, Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _database = database;
            _client = client;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public async Task<User> ReadOrCreateUser(string userId)
        {
            var user = _client.CreateUserQuery("dbs/" + _database.ResourceId + "/users/")
                    .AsEnumerable()
                    .FirstOrDefault(u => u.Id == userId);

            // If user does not exists, create it
            if (user == null)
            {
                await _client.CreateUserAsync(_database.SelfLink, new User { Id = userId });
            }

            return user;
        }

        /// <summary>
        /// Creates a permission with an access token for the specified user and the speciefied collection
        /// </summary>
        public async Task<Permission> CreateUserPermission(User user, DocumentCollection collection, PermissionMode permission)
        {
            string permissionId = permission + collection.Id;

            // The permission may already exists on database, try to find it
            Permission collectionPermission = _client.
                CreatePermissionQuery("/dbs/" + _database.ResourceId + "/users/" + user.ResourceId + "/permissions").
                AsEnumerable().FirstOrDefault(u => u.Id == permissionId);

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

            return await _client.CreatePermissionAsync(user.SelfLink, collectionPermission);
        }
    }
}
