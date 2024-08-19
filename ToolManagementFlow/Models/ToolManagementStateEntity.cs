using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolManagementFlow.Models
{
    public class ToolManagementStateEntity
    {
        public HashSet<string> CurrentTools { get; set; }

        internal List<IToolDefinition> tools = new List<IToolDefinition>();

    }
}
