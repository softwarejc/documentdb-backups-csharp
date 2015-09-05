using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace DocumentDB.Framework.Interfaces
{
    public interface IUsersService
    {
        /// <summary>
        /// Creates a user.
        /// </summary>
        Task<User> ReadOrCreateUser(string userid);

        /// <summary>
        /// Creates a user permission.
        /// </summary>
        Task<Permission> CreateUserPermission(User user, DocumentCollection collection, PermissionMode permission);
    }
}