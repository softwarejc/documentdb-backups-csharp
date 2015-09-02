using System;
using System.Configuration;
using DocumentDb_HelloWorld.Common;

namespace DocumentDb_HelloWorld.Domain
{
    class MyFoodContext : IDisposable
    {
        public IDocumentDbCollection<Item> ShoppingList { get; }

        public MyFoodContext(string databaseId = null)
        {
            if (string.IsNullOrEmpty(databaseId))
            {
                databaseId = ConfigurationManager.AppSettings["database"];
            }

            ShoppingList = new DocumentDbCollection<Item>(databaseId, ConfigurationManager.AppSettings["shoppingListCollectionId"]);
        }

        public void Dispose()
        {
            ShoppingList.Dispose();
        }
    }
}
