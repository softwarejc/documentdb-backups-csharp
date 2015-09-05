using System;
using System.Configuration;

using DocumentDB.Framework;
using DocumentDB.Framework.Interfaces;

namespace DocumentDb_HelloWorld.Domain
{
    public class MyFoodContext : DocumentDBContext
    {
        public MyFoodContext()
            : base(
                new Uri(ConfigurationManager.AppSettings["endPointUrl"]),
                ConfigurationManager.AppSettings["authorizationKey"],
                ConfigurationManager.AppSettings["database"])
        {
            ShoppingList = DocumentDBCollection<Item>("ShoppingListCollection").Result;
        }

        public IDocumentDBCollection<Item> ShoppingList { get; }
    }
}