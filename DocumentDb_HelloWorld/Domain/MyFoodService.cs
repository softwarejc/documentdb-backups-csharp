using System;
using System.Configuration;

using DocumentDB.Framework;
using DocumentDB.Framework.Interfaces;

namespace DocumentDb_HelloWorld.Domain
{
    public class MyFoodService : DocumentDBService
    {
        private const string ShoppingListCollectionId = "ShoppingListCollection";

        public MyFoodService()
            : base(
                ConfigurationManager.AppSettings["endPointUrl"],
                ConfigurationManager.AppSettings["authorizationKey"],
                ConfigurationManager.AppSettings["database"])
        {
            ShoppingList = CreateCollectionService<Item>(ShoppingListCollectionId).Result;
        }

        public ICollectionService<Item> ShoppingList { get; }
    }
}