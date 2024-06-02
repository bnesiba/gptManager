using Newtonsoft.Json;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions.Models
{
    public interface IToolDefinition
    {
        public string Name { get; }
        public string Description { get; }

        public List<ToolProperty> InputParameters { get; }

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall);

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolRequestParameters);

    }

    public static class ToolDefinitionExtensions
    {
        public static OpenAITool GetToolRequestDefinition(this IToolDefinition toolDefinition)
        {
            Dictionary<string, object> toolProperties = toolDefinition.InputParameters.Where(p => p.type != "array")
                .ToDictionary(p => p.name, p => new { p.type, p.description } as object);

            if (toolDefinition.InputParameters.Any(p => p.type == "array"))
            {
                var arrayTools = toolDefinition.InputParameters.Where(p => p.type == "array").ToList();
                foreach (var arrayTool in arrayTools)
                {
                    var arTool = arrayTool as ArrayToolProperty;
                    toolProperties.Add(arTool.name, new { arTool.type, arTool.description, arTool.items });
                }
            }

            OpenAiToolFunction toolFunction = new OpenAiToolFunction()
            {
                description = toolDefinition.Description,
                name = toolDefinition.Name,
                parameters = new
                {
                    type = "object",
                    properties = toolDefinition.InputParameters.Where(p => p.type != "array").ToDictionary(p => p.name, p => new { p.type, p.description }),
                    required = toolDefinition.InputParameters.Where(p => p.IsRequired).Select(p => p.name).ToArray()
                }
            };

            return new OpenAITool()
            {
                type = "function",
                function = toolFunction
            };
        }

        public static Dictionary<string, string>? GetToolRequestStringParameters(this IToolDefinition toolDefinition,
            OpenAIToolCall toolCall)
        {
            Dictionary<string, object>? requestParameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(toolCall.function.arguments);

            Dictionary<string, string> resultsDictionary = new Dictionary<string, string>();
            if (requestParameters != null)
            {
                toolDefinition.InputParameters.Where(p => p.type != "array").ToList().ForEach(p =>
                {
                    string? valueString = requestParameters[p.name].ToString();
                    if (valueString != null)
                    {
                        resultsDictionary.Add(p.name, valueString);
                    }
                });

            }

            return resultsDictionary;
        }

        public static Dictionary<string, List<string>>? GetToolRequestArrayParameters(this IToolDefinition toolDefinition,
            OpenAIToolCall toolCall)
        {
            Dictionary<string, object>? requestParameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(toolCall.function.arguments);

            Dictionary<string, List<string>> resultsDictionary = new Dictionary<string, List<string>>();
            if (requestParameters != null)
            {
                toolDefinition.InputParameters.Where(p => p.type == "array").ToList().ForEach(p =>
                {
                    if (requestParameters.ContainsKey(p.name))
                    {
                        var arrayThing = JsonConvert.DeserializeObject<List<string>>(requestParameters[p.name].ToString() ?? string.Empty);
                        if (arrayThing != null)
                        {
                            resultsDictionary.Add(p.name, arrayThing);
                        }
                    }
                });
            }
            return resultsDictionary;
        }

        public static bool RequestArgumentsValid<T>(this IToolDefinition toolDefinition,
            Dictionary<string, T>? requestParameters)
        {
            if (requestParameters == null)
            {
                return false;
            }

            foreach (var parameter in toolDefinition.InputParameters)
            {
                if (parameter.IsRequired && !requestParameters.ContainsKey(parameter.name))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool RequestArgumentsValid<T1, T2>(this IToolDefinition toolDefinition,
            Dictionary<string, T1>? requestStringParameters, Dictionary<string, T2>? requestArrayParameters)
        {
            //I think this is handled by the code below. If both are null the foreach checks will return false unles there aren't any required parameters.
            //if (requestStringParameters == null && requestArrayParameters == null)
            //{
            //    return false;
            //}

            foreach (var parameter in toolDefinition.InputParameters)
            {
                if (parameter.IsRequired)
                {
                    if ((requestStringParameters == null || !requestStringParameters.ContainsKey(parameter.name))
                        && (requestArrayParameters == null || !requestArrayParameters.ContainsKey(parameter.name)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public static class ToolArrayExtensions
    {
        public static OpenAITool[] GetToolDefinitions(this List<IToolDefinition> tools)
        {
            return tools.Select(t => t.GetToolRequestDefinition()).ToArray();
        }

        public static List<OpenAIToolMessage> ExecuteTools(this List<IToolDefinition> tools, List<OpenAIChatMessage> chatContext, List<OpenAIToolCall> toolCalls)
        {
            var invalidToolCalls = toolCalls.Where(t => tools.All(tf => tf.Name != t.function.name)).ToList();
            if (invalidToolCalls.Any())
            {
                //throw new Exception($"Invalid tool call(s): {string.Join(", ", invalidToolCalls.Select(t => t.function.name))}");
                Console.WriteLine($"WARNING There were invalid tool calls:{string.Join(", ", invalidToolCalls.Select(t => t.function.name))}");
            }

            var results = new List<OpenAIToolMessage>();
            foreach (var toolCall in toolCalls)
            {
                var tool = tools.FirstOrDefault(t => t.Name == toolCall.function.name);
                if (tool != null)
                {
                    results.Add(tool.ExecuteTool(chatContext, toolCall));
                }
            }
            return results;
        }


    }
}
