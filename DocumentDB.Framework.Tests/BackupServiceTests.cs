using System;
using System.Linq;

using DocumentDB.Framework.Tests.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentDB.Framework.Tests.Backups
{
    [TestClass]
    public class BackupServiceTests : DocumentDBTestsBase
    {
        private DocumentDBServiceAccessor _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _service = new DocumentDBServiceAccessor(
                UnitTestsConfig.EndPoint,
                UnitTestsConfig.AuthorizationKey,
                UnitTestsConfig.Database);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _service.Dispose();
        }

        [TestMethod]
        public void CreateBackupTest()
        {
            // Arrange
            var backupService = _service.GetBackupService();

            // Act
            backupService.CreateBackup("backup1");
            backupService.CreateBackup("backup2");

            // Assert
            // two backups are created
            Assert.AreEqual(2, backupService.GetAvailableBackups().Count());
        }

        [TestMethod]
        public void GetBackupTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void RestoreBackupTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DeleteBackupTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetAvailableBackupsTest()
        {
            Assert.Fail();
        }
    }
}