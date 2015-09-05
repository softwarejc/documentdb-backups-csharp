using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace DocumentDB.Framework.Interfaces
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Gets the database associated to this service.
        /// </summary>
        Database Database { get; }

        /// <summary>
        /// Deletes the service database.
        /// </summary>
        /// <returns></returns>
        Task Delete();
    }
}