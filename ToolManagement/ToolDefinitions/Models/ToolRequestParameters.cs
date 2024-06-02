using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolManagement.ToolDefinitions.Models
{
    public class ToolRequestParameters
    {
        public string ToolRequestId { get; set; }
        private Dictionary<string,string>? _stringParameters { get; set; }
        private Dictionary<string, List<string>>? _arrayParameters { get; set; }

        public ToolRequestParameters(string id, Dictionary<string,string>? strParams, Dictionary<string, List<string>>? arrayParams)
        {
            ToolRequestId = id;
            _stringParameters = strParams;
            _arrayParameters = arrayParams;
        }

        public string GetStringParam(string key)
        {
            if (this._stringParameters == null) return null;
            return this._stringParameters.ContainsKey(key) ? this._stringParameters[key] : null;
        }

        public List<string> GetArrayParam(string key)
        {
            if (this._arrayParameters == null) return null;
            return this._arrayParameters.ContainsKey(key) ? this._arrayParameters[key] : null;
        }

    }
}
