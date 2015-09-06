using System;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;

namespace DocumentDB.Framework.Database
{
    /// <summary>
    ///     Service to perform DocumentDB actions.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        ///     Deletes the service database.
        /// </summary>
        Task DeleteDatabase();

        /// <summary>
        ///     Reads or create a collection.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        Task<DocumentCollection> ReadOrCreateCollection(string collectionId);

        /// <summary>
        ///     Deletes a collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        Task DeleteCollection(DocumentCollection collection);

        /// <summary>
        ///     Reads or create a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        Task<User> ReadOrCreateUser(string userId);

        /// <summary>
        ///     Creates a user permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="permission">The permission.</param>
        Task<Permission> CreateUserPermission(User user, DocumentCollection collection, PermissionMode permission);

        /// <summary>
        /// Gets the database URI.
        /// </summary>
        Uri DatabaseUri { get; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        Microsoft.Azure.Documents.Database Database { get; }
    }
}