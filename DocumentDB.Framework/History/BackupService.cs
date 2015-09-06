using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DocumentDB.Framework.Collections;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DocumentDB.Framework.Backups
{
    public class BackupService : IBackupService
    {
        private readonly ICollectionService<Backup> _backupService;

        private readonly ICollectionService<Document> _sourceService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BackupService" /> class.
        /// </summary>
        public BackupService(ICollectionService<Document> sourceService, ICollectionService<Backup> backupService)
        {
            if (sourceService == null)
            {
                throw new ArgumentNullException(nameof(sourceService));
            }
            if (backupService == null)
            {
                throw new ArgumentNullException(nameof(backupService));
            }

            _sourceService = sourceService;
            _backupService = backupService;
        }

        /// <summary>
        ///     Creates a new backup.
        /// </summary>
        public async Task<Backup> CreateBackup(string description)
        {
            // Get documents to backup
            var backupContent = _sourceService.AllDocuments;

            // Create backup
            var backup = new Backup { Description = description, Content = backupContent.ToList() };

            return await _backupService.CreateDocument(backup);
        }

        /// <summary>
        ///     Gets a backup.
        /// </summary>
        public async Task<Backup> GetBackup(string backupLink)
        {
            return await _backupService.GetDocumentByLink(backupLink);
        }

        /// <summary>
        ///     Restores a backup.
        /// </summary>
        public async Task RestoreBackup(Backup backup)
        {
            var collectionId = _sourceService.Collection.Id;

            // DocumentDB does not support bulk delete yet, as this service only does documents backup
            // delete the collection and create a new one with the backup content
            await _sourceService.DatabaseService.DeleteCollection(_sourceService.Collection);

            // Create new collection
            await _sourceService.DatabaseService.ReadOrCreateCollection(collectionId);

            // Add backed-up documents to the collection
            backup.Content.ForEach(async document => await _sourceService.CreateDocument(document));
        }

        /// <summary>
        ///     Deletes a backup.
        /// </summary>
        /// <param name="backupLink">The link of the backup to delete.</param>
        public async Task DeleteBackup(string backupLink)
        {
            await _backupService.DeleteDocument(backupLink);
        }

        /// <summary>
        ///     Gets the available backups. The content of the backup shall not be included.
        /// </summary>
        public IEnumerable<BackupInfo> GetAvailableBackups()
        {
            // Link to backup documents
            return
                _backupService.Client.CreateDocumentQuery<Backup>(_backupService.CollectionUri)
                    .Select(backup => new BackupInfo(backup.Id, backup.SelfLink, backup.Description, backup.Timestamp))
                    .AsEnumerable();
        }
    }
}