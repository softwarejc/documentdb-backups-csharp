using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDb_HelloWorld.Common
{
    public class DocumentDbUserHelper
    {
        /// <summary>
        /// Gets a user by id
        /// </summary>
        public User GetUser(DocumentClient client, Database database, string userId)
        {
            return client.CreateUserQuery("dbs/" + database.ResourceId + "/users/")
                    .AsEnumerable()
                    .FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public async Task<User> CreateUser(string userid, DocumentClient client, Database database)
        {
            var user = new User { Id = userid };

            // Check if user already exists
            user = GetUser(client, database, user.Id) ?? await client.CreateUserAsync(database.SelfLink, user);

            return user;
        }

        /// <summary>
        /// Creates a permission with an access token for the specified user and the speciefied collection
        /// By default this access is valid for 1 hour
        /// </summary>
        public async Task<Permission> CreateUserPermission(DocumentClient client, Database database, User user, DocumentCollection collection, PermissionMode permission)
        {
            string permissionId = permission + collection.Id;

            // The permission may already exists on database, try to find it
            Permission collectionPermission = client.
                CreatePermissionQuery("/dbs/" + database.ResourceId + "/users/" + user.ResourceId + "/permissions").
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

            return await client.CreatePermissionAsync(user.SelfLink, collectionPermission);
        }

        /// <summary>
        /// Creates a document db client using the specified permission token
        /// </summary>
        public DocumentClient CreateClient(Uri endPoint, Permission permission)
        {
            return new DocumentClient(endPoint, permission.Token);
        }
    }
}
