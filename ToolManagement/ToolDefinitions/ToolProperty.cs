using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ToolManagement.ToolDefinitions
{
    public class ToolProperty
    {
        public string name { get; init; }
        public string type { get; init; }
        public string description { get; init; }

        [IgnoreDataMember]
        public bool IsRequired { get; init; }
    }
}
