using DocumentDB.Framework.Backups;

using Microsoft.Azure.Documents;

using Newtonsoft.Json;

namespace DocumentDB.Framework.Tests.Helpers
{
    internal class Item : Document
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    internal class DocumentDBServiceAccessor : DocumentDBService
    {
        public DocumentDBServiceAccessor(string endPointUrl, string authorizationKey, string databaseId)
            : base(endPointUrl, authorizationKey, databaseId)
        {
        }

        public IBackupService GetBackupService()
        {
            var collection = CreateCollectionService<Item>("unitTestsCollection").Result;

            // Add a document to create backups from it
            collection.CreateDocument(new Item { Name = "ImportantData_1" });
            collection.CreateDocument(new Item { Name = "ImportantData_2" });

            return CreateCollectionBackupService(collection);
        }
    }
}