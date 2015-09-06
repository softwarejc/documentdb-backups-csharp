using System;
using System.Diagnostics;

using DocumentDB.Framework.Database;

using Microsoft.Azure.Documents.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentDB.Framework.Tests.Helpers
{
    [TestClass]
    public class DocumentDBTestsBase
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Debug.WriteLine("AssemblyInitialize -> Create database");

            // Not needed because it will be automatically created
            // CreateDatabase(); 
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup -> Delete database");

            DeleteDatabase();
        }

        protected static void CreateDatabase()
        {
            using (var client = new DocumentClient(new Uri(UnitTestsConfig.EndPoint), UnitTestsConfig.AuthorizationKey))
            {
                DatabaseService.ReadOrCreateDatabase(client, UnitTestsConfig.Database).Wait();
            }
        }

        protected static void DeleteDatabase()
        {
            using (var client = new DocumentClient(new Uri(UnitTestsConfig.EndPoint), UnitTestsConfig.AuthorizationKey))
            {
                DatabaseService.DeleteDatabase(client, UnitTestsConfig.Database).Wait();
            }
        }
    }
}