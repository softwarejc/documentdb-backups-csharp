using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB.Portable
{
    public class DocumentDBContext : IDisposable
    {
        protected DocumentDBContext(string endPoint, string authToken, string database)
        {
            // client to access the database
            Client = new DocumentClient(new Uri(endPoint), authToken);
            Database = ReadOrCreateDatabase(Client, database).Result;
        }

        protected DocumentClient Client { get; }
        protected Database Database { get; }

        public virtual void Dispose()
        {
            Client.Dispose();
        }

        private static async Task<Database> ReadOrCreateDatabase(DocumentClient client, string databaseId)
        {
            // Get the database
            var db = client.CreateDatabaseQuery()
                .Where(d => d.Id == databaseId)
                .AsEnumerable()
                .FirstOrDefault();

            // Return the database or create it if not exists yet
            return db ?? (await client.CreateDatabaseAsync(new Database { Id = databaseId }));
        }
    }
}