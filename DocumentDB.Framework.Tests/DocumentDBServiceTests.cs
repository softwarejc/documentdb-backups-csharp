using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace DocumentDB.Framework.Tests
{
    [TestClass()]
    public class DocumentDBTestsBase
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Console.WriteLine("AssemblyInitialize -> DocumentDBServiceTests");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup -> DocumentDBServiceTests");
        }
    }

    [TestClass()]
    public class DocumentDBServiceTests: DocumentDBTestsBase
    {
        [TestMethod]
        public void DocumentDBServiceTests1111()
        {
            Assert.Fail();
        }
    }
}