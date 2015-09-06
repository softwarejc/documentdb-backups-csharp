using System;

namespace DocumentDB.Framework.Backups
{
    /// <summary>
    /// </summary>
    public class BackupInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BackupInfo" /> class.
        /// </summary>
        public BackupInfo(string id, string selfLink, string description, DateTime timestamp)
        {
            Id = id;
            SelfLink = selfLink;
            Description = description;
            Timestamp = timestamp;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets the self link.
        /// </summary>
        public string SelfLink { get; }

        /// <summary>
        ///     Gets the backup description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Gets the backup creation date.
        /// </summary>
        public DateTime Timestamp { get; }
    }
}