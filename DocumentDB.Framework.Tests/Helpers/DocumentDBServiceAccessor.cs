using DocumentDB.Framework.Collections;
using DocumentDB.Framework.History;

using Microsoft.Azure.Documents;

using Newtonsoft.Json;

namespace DocumentDB.Framework.Tests.Helpers
{
    public class Item : Document
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

        public ICollectionService<Item> SampleData { get; set; }

        public IBackupService GetBackupService(int sampleDocumentsToCreateInCollectionSource = 1)
        {
            SampleData = CreateCollectionService<Item>("unitTestsCollection").Result;

            for (var i = 0; i < sampleDocumentsToCreateInCollectionSource; i++)
            {
                // Add a document to create backups from it
                SampleData.CreateDocument(new Item { Name = "ImportantData_" + i });
            }

            return CreateCollectionBackupService(SampleData);
        }
    }
}