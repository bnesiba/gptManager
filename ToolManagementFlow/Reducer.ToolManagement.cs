using ActionFlow.Models;
using ToolManagementFlow.Models;


namespace ToolManagementFlow
{
    //Reducer
    public class ToolManagementReducer : IFlowStateReducer<ToolManagementStateEntity>
    {
        private List<IToolDefinition> _toolDefinitions;
        public ToolManagementReducer(IEnumerable<IToolDefinition> definedTools)
        {
            _toolDefinitions = definedTools.ToList();
        }

        public ToolManagementStateEntity InitialState => new ToolManagementStateEntity();

        public List<IFlowReductionBase<ToolManagementStateEntity>> Reductions => new List<IFlowReductionBase<ToolManagementStateEntity>>
        {
            this.reduce(CurrentTools_OnSetCurrentToolset_SetCurrentTools, ToolManagementActions.SetToolset()),
        };

        

        //Reducer Methods
        public ToolManagementStateEntity CurrentTools_OnSetCurrentToolset_SetCurrentTools(FlowAction<List<string>> setToolsAction,
            ToolManagementStateEntity currentState)
        {
            currentState.CurrentTools = setToolsAction.Parameters.ToHashSet<string>();
            currentState.tools = _toolDefinitions;

            return currentState;
        }


    }
}
