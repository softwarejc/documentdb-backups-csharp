using System;

namespace DocumentDB.Framework.History
{
    /// <summary>
    /// </summary>
    public class BackupInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BackupInfo" /> class.
        /// </summary>
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets the self link.
        /// </summary>
        public string SelfLink { get; set; }

        /// <summary>
        ///     Gets the backup description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets the backup creation date.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}