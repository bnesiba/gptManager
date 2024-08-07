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

        public Dictionary<string, object>? ObjectParameters { get; set; }
        public Dictionary<string, List<string>>? StringArrayParameters { get; set; }
        public Dictionary<string, List<object>>? ObjectArrayParameters { get; set; }

        public ToolRequestParameters(string name, string id, Dictionary<string,string>? strParams,Dictionary<string, object>? objectParameters,  Dictionary<string, List<string>>? stringArrayParams, Dictionary<string, List<object>>? objectArrayParameters)
        {
            ToolName = name;
            ToolRequestId = id;
            StringParameters = strParams;
            ObjectParameters = objectParameters;
            StringArrayParameters = stringArrayParams;
            ObjectArrayParameters = objectArrayParameters;
        }

        public string? GetStringParam(string key)
        {
            if (this.StringParameters == null) return null;
            return this.StringParameters.ContainsKey(key) ? this.StringParameters[key] : null;
        }

        public T? GetObjectParam<T>(string key)
        {
            if (this.ObjectParameters == null) return default(T);
            return this.ObjectParameters.ContainsKey(key) ? (T)this.ObjectParameters[key]: default(T);
        }

        public List<string>? GetStringArrayParam(string key)
        {
            if (this.StringArrayParameters == null) return null;
            return this.StringArrayParameters.ContainsKey(key) ? this.StringArrayParameters[key] : null;
        }

        public List<T>? GetObjectArrayParam<T>(string key)
        {
            if (this.ObjectArrayParameters == null) return null;
            return this.ObjectArrayParameters.ContainsKey(key) ? this.ObjectArrayParameters[key] as List<T> : null;
        }
    }
}
