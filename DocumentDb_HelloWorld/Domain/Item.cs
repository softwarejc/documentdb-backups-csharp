using Microsoft.Azure.Documents;

using Newtonsoft.Json;

namespace DocumentDb_HelloWorld.Domain
{
    public class Item : Document
    {
        [JsonProperty(PropertyName = "type")]
        public string Type => "item";

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}