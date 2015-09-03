using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using DocumentDb_HelloWorld.Common;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDb_HelloWorld.Domain
{
    class MyFoodContext : IDisposable
    {
        public MyFoodContext()
        {
            // client to access the database
            var client = new DocumentClient(
                serviceEndpoint: new Uri(ConfigurationManager.AppSettings["endPointUrl"]),
                authKeyOrResourceToken: ConfigurationManager.AppSettings["authorizationKey"]);

            Database = ReadOrCreateDatabase(client, ConfigurationManager.AppSettings["database"]).Result;

            ShoppingList = new DocumentDbCollection<Item>(Database, ConfigurationManager.AppSettings["shoppingListCollectionId"], client);
        }

        public IDocumentDbCollection<Item> ShoppingList { get; }

        public Database Database { get; }

        public void Dispose()
        {
            ShoppingList.Dispose();
        }

        // todo move to helper class
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
