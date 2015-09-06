using System.Configuration;

using DocumentDB.Framework;
using DocumentDB.Framework.Backups;
using DocumentDB.Framework.Collections;

namespace DocumentDb_HelloWorld.Domain
{
    public class FoodService : DocumentDBService
    {
        public FoodService()
            : base(
                ConfigurationManager.AppSettings["endPointUrl"],
                ConfigurationManager.AppSettings["authorizationKey"],
                ConfigurationManager.AppSettings["database"])
        {
            // Create collection service
            ShoppingList = CreateCollectionService<Item>(ShoppingListCollectionId).Result;

            // Create a backup service for the shopping list collection
            BackupService = CreateCollectionBackupService(ShoppingList);
        }

        private const string ShoppingListCollectionId = "ShoppingListCollection";

        public ICollectionService<Item> ShoppingList { get; }

        public IBackupService BackupService { get; }
    }
}