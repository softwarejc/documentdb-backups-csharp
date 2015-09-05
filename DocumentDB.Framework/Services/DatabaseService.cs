using System.Linq;
using System.Threading.Tasks;

using DocumentDB.Framework.Interfaces;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Framework.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DocumentClient _client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseService" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="databaseId">The database identifier.</param>
        public DatabaseService(DocumentClient client, string databaseId)
        {
            _client = client;
            Database = ReadOrCreateDatabase(databaseId).Result;
        }

        /// <summary>
        ///     Gets the database associated to this service.
        /// </summary>
        public Database Database { get; }

        /// <summary>
        ///     Deletes the service database.
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _client.DeleteDatabaseAsync(Database.SelfLink);
        }

        /// <summary>
        ///     Reads or create a database.
        /// </summary>
        /// <param name="databaseId">The database identifier.</param>
        private async Task<Database> ReadOrCreateDatabase(string databaseId)
        {
            // Get the database
            var db = _client.CreateDatabaseQuery().Where(d => d.Id == databaseId).AsEnumerable().FirstOrDefault();

            // Return the database or create it if not exists yet
            return db ?? await _client.CreateDatabaseAsync(new Database { Id = databaseId });
        }
    }
}