using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;

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

    public class ArrayToolProperty : ToolProperty
    {
        public ToolProperty items { get; init; }
    }

    public class EnumToolProperty : ToolProperty
    {
        [JsonPropertyName("enum")]
        public List<string> enumValues { get; set; }
    }
}
