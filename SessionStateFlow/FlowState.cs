using SessionStateFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow
{
    public class FlowState
    {
        private delegate void FlowStateActions(IFlowAction action);
        private FlowStateActions _flowStateActions = (action) => {Console.WriteLine($"action: {action.Name}\n");};

        public void ResolveAction(IFlowAction action)
        {
            _flowStateActions(action);
        }

        public void RegisterEffect(Predicate<IFlowAction> filter, Func<IFlowAction, IFlowAction> effect)
        {
            _flowStateActions += (flowAction) =>
            {
                if (filter(flowAction))
                {
                    _flowStateActions(effect(flowAction));
                }
            };
        }

        //provided predicates?
        public static Predicate<IFlowAction> onAction(params IFlowAction[] actions)
        {
            return (flowAction) => actions.Any(a => a.GetType() == flowAction.GetType());
        }

        private class EmptyAction : IFlowAction
        {
            public string Name => "EmptyAction";
        }
    }
}
