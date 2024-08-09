using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using Newtonsoft.Json;

namespace ToolManagement.ToolDefinitions.Models
{
    public class ToolProperty
    {
        public string name { get; init; }
        public string type { get; init; }
        public string description { get; init; }

        [IgnoreDataMember]
        public bool IsRequired { get; init; }
    }

    public class ObjectToolProperty : ToolProperty
    {
        public List<ToolProperty> properties { get; init; }
    }

    public class ArrayToolProperty : ToolProperty
    {
        public ToolProperty items { get; init; }
    }

    public class EnumToolProperty : ToolProperty
    {
        [JsonProperty("enum")]
        public List<string> enumValues { get; set; }
    }

    public class TestyToolProperty: ToolProperty
    {
        public bool tested { get; init; }
    }

    public class ToolParameter
    {
        public string type { get; init; }
        public string description { get; init; }
    }

    public class ArrayToolParameter : ToolParameter
    {
        public ToolParameter items { get; init; }
    }

    public class EnumToolParameter : ToolParameter
    {
        [JsonProperty("enum")]
        public List<string> enumValues { get; init; }
    }

    public class ObjectToolParameter : ToolParameter
    {
        public Dictionary<string, ToolParameter> properties { get; init; }
        public bool additionalProperties { get; set; } = false; //TODO: figure out -> required for object parameters using strict i think? (this could be for optional properties maybe?)
        
        public string[] required { get; init; }

    }
}
