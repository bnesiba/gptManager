using Newtonsoft.Json;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions.Models
{
    public interface IToolDefinition
    {
        public string Name { get; }
        public string Description { get; }

        public List<ToolProperty> InputParameters { get; }

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolRequestParameters);

    }

    public static class ToolDefinitionExtensions
    {
        public static KeyValuePair<string, object> GetInputParametersObject(this ToolProperty toolProperty)
        {
            if(toolProperty is ArrayToolProperty)
            {
                ArrayToolProperty arrProp = (ArrayToolProperty)toolProperty;
                return new KeyValuePair<string, object>(toolProperty.name, new ArrayToolParameter
                {
                    description = arrProp.description,
                    type = arrProp.type,
                    items = new ToolParameter { type = arrProp.items.type}
                });
            }
            if(toolProperty is EnumToolProperty)
            {
                EnumToolProperty enumToolProperty = (EnumToolProperty)toolProperty;
                return new KeyValuePair<string, object>(toolProperty.name, new EnumToolParameter
                {
                    type = enumToolProperty.type,
                    description = enumToolProperty.description,
                    enumValues = enumToolProperty.enumValues
                });
            }
            return new KeyValuePair<string, object>(toolProperty.name, new ToolParameter { type = toolProperty.type, description = toolProperty.description });
        }
        public static OpenAITool GetToolRequestDefinition(this IToolDefinition toolDefinition)
        {
            Dictionary<string, object> toolParameters = toolDefinition.InputParameters.Select(p => GetInputParametersObject(p)).ToDictionary();

            OpenAiToolFunction toolFunction = new OpenAiToolFunction()
            {
                description = toolDefinition.Description,
                name = toolDefinition.Name,
                parameters = new
                {
                    type = "object",
                    properties = toolParameters,
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
    }
}
