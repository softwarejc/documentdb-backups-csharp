using System;
using System.Linq;
using System.Threading;

using DocumentDB.Framework.Tests.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentDB.Framework.Tests.History
{
    [TestClass]
    public class BackupServiceTests : DocumentDBTestsBase
    {
        private DocumentDBServiceAccessor _service;

        [TestInitialize]
        public void TestInitialize()
        {
            DeleteDatabase();
            _service = new DocumentDBServiceAccessor(UnitTestsConfig.EndPoint, UnitTestsConfig.AuthorizationKey, UnitTestsConfig.Database);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _service.Dispose();
            DeleteDatabase();

            // Avoid to much Azure DocumentDB requests per second
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void CreateBackupTest()
        {
            // Arrange
            var backupService = _service.GetBackupService();

            // Act
            backupService.CreateBackup("backup1").Wait();
            backupService.CreateBackup("backup2").Wait();

            // Assert

            // 1) two backups are created
            var backupsInfo = backupService.GetAvailableBackups().ToList();
            Assert.AreEqual(2, backupsInfo.Count);
        }

        [TestMethod]
        public void GetBackupTest()
        {
            // Arrange 
            var itemsToCreate = 3;
            var lastBackupName = "lastBackup!";

            var backupService = _service.GetBackupService(itemsToCreate);
            backupService.CreateBackup("backup1").Wait();
            backupService.CreateBackup(lastBackupName).Wait();

            // Act
            // backup info has no content
            var lastBackupInfo = backupService.GetAvailableBackups().Last();
            // backup with content
            var lastBackup = backupService.GetBackup(lastBackupInfo.SelfLink).Result;

            // Assert
            Assert.AreEqual(lastBackupName, lastBackup.Description);
            Assert.AreEqual(lastBackupInfo.Timestamp, lastBackup.Timestamp);
            Assert.AreEqual(itemsToCreate, lastBackup.Content.Count);
        }

        [TestMethod]
        public void RestoreBackupTest()
        {
            // Arrange 
            var itemsToCreate = 3;
            var backupName = "MyBackup";

            var backupService = _service.GetBackupService(itemsToCreate);
            backupService.CreateBackup(backupName).Wait();

            // Act

            // get all items
            var allItems = _service.SampleData.AllDocuments.ToList();
            // we have 3 items
            Assert.AreEqual(itemsToCreate, allItems.Count);

            // delete item in source collection
            var firstItem = allItems.FirstOrDefault();
            Assert.IsNotNull(firstItem);
            _service.SampleData.DeleteDocument(firstItem.SelfLink);

            // now we have 2 items
            allItems = _service.SampleData.AllDocuments.ToList();
            Assert.AreEqual(itemsToCreate - 1, allItems.Count);

            // restore a backup
            // backup info has no content
            var lastBackupInfo = backupService.GetAvailableBackups().Last();

            // backup with content
            var lastBackup = backupService.GetBackup(lastBackupInfo.SelfLink).Result;
            backupService.RestoreBackup(lastBackup).Wait();

            // Assert
            // the source collection has again all the documents 
            // get all items
            allItems = _service.SampleData.AllDocuments.ToList();
            // we have 3 items
            Assert.AreEqual(itemsToCreate, allItems.Count);
        }

        [TestMethod]
        public void DeleteBackupTest()
        {
            // Arrange 
            var backupService = _service.GetBackupService();
            backupService.CreateBackup("backup1").Wait();
            backupService.CreateBackup("backup2").Wait();

            // Act
            // get last backupInfo
            var lastBackupInfo = backupService.GetAvailableBackups().Last();

            // delete last backup
            backupService.DeleteBackup(lastBackupInfo.SelfLink).Wait();

            // Assert
            Assert.AreEqual(1, backupService.GetAvailableBackups().Count());
        }
    }
}