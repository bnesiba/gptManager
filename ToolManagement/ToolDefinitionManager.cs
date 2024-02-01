using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions;

namespace ToolManagement
{
    public class ToolDefinitionManager
    {
       private List<ToolDefinition> tools = new List<ToolDefinition>();

       public ToolDefinitionManager()
       {
            tools.Add(new UserQuery());
            tools.Add(new InternetSearch());
            tools.Add(new FindEmails());
       }

        public OpenAITool[] GetToolDefinitions()
        {
            return tools.Select(t => t.GetToolRequestDefinition()).ToArray();
        }
    }
}
