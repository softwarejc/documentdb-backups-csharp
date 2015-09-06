using System.Collections.Generic;

using Microsoft.Azure.Documents;

using Newtonsoft.Json;

namespace DocumentDB.Framework.Backups
{
    /// <summary>
    ///     Backup object
    /// </summary>
    public class Backup : Document
    { 
        /// <summary>
        ///     Gets the document type.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type => "backup";

        /// <summary>
        ///     Gets or sets a description of this backup.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the content of the backup.
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public List<Document> Content { get; set; }
    }
}