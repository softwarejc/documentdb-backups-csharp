using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentDB.Framework.History
{
    public interface IBackupService
    {
        /// <summary>
        ///     Creates a new backup.
        /// </summary>
        Task<Backup> CreateBackup(string description);

        /// <summary>
        ///     Gets a backup.
        /// </summary>
        Task<Backup> GetBackup(string backupLink);

        /// <summary>
        ///     Restores a backup.
        /// </summary>
        /// <param name="backup">The backup to restore.</param>
        Task RestoreBackup(Backup backup);

        /// <summary>
        ///     Deletes the backup.
        /// </summary>
        /// <param name="backupLink">The link of the backup to delete.</param>
        Task DeleteBackup(string backupLink);

        /// <summary>
        ///     Gets the available backups. The content of the backup shall not be included.
        /// </summary>
        IEnumerable<BackupInfo> GetAvailableBackups();
    }
}