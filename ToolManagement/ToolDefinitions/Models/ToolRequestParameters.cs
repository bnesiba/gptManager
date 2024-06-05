using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolManagement.ToolDefinitions.Models
{
    public class ToolRequestParameters
    {
        public string ToolName { get; set; }
        public string ToolRequestId { get; set; }
        public Dictionary<string,string>? StringParameters { get; set; }
        public Dictionary<string, List<string>>? ArrayParameters { get; set; }

        public ToolRequestParameters(string name, string id, Dictionary<string,string>? strParams, Dictionary<string, List<string>>? arrayParams)
        {
            ToolName = name;
            ToolRequestId = id;
            StringParameters = strParams;
            ArrayParameters = arrayParams;
        }

        public string GetStringParam(string key)
        {
            if (this.StringParameters == null) return null;
            return this.StringParameters.ContainsKey(key) ? this.StringParameters[key] : null;
        }

        public List<string> GetArrayParam(string key)
        {
            if (this.ArrayParameters == null) return null;
            return this.ArrayParameters.ContainsKey(key) ? this.ArrayParameters[key] : null;
        }

    }
}
