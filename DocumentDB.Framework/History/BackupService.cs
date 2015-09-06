using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DocumentDB.Framework.Collections;

using Microsoft.Azure.Documents;

namespace DocumentDB.Framework.History
{
    public class BackupService<T> : IBackupService
        where T : Document
    {
        private readonly ICollectionService<Backup> _backupService;

        private readonly ICollectionService<T> _sourceService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BackupService{T}" /> class.
        /// </summary>
        /// <param name="sourceService">The source service.</param>
        /// <param name="backupService">The backup service.</param>
        public BackupService(ICollectionService<T> sourceService, ICollectionService<Backup> backupService)
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
            var backupContent = _sourceService.AllDocuments.Cast<Document>().ToList();

            // Create backup
            var backup = new Backup { Description = description, Content = backupContent };

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
            // DocumentDB does not support bulk delete yet, delete 1 by 1
            foreach (var document in _sourceService.AllDocuments)
            {
                await _sourceService.DeleteDocument(document.SelfLink);
            }

            // Add backed-up documents to the collection
            foreach (var document in backup.Content)
            {
                T typedDocument = (dynamic)document;

                await _sourceService.CreateDocument(typedDocument);
            }
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
            return
                _backupService.Client.CreateDocumentQuery<Backup>(_backupService.CollectionUri)
                    .AsEnumerable()
                    .Select(
                        backup =>
                        new BackupInfo { Id = backup.Id, SelfLink = backup.SelfLink, Description = backup.Description, Timestamp = backup.Timestamp });
        }
    }
}