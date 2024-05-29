using Newtonsoft.Json;
using SessionStateFlow.package.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SessionStateFlow.package
{
    //TODO: will probably need to break up some of the logic in this file eventually
    public class FlowState
    {
        internal delegate void FlowStateActions(FlowActionBase action);
        internal FlowStateActions _flowStateActions = (action) => { System.Diagnostics.Debug.WriteLine($"action: {JsonConvert.SerializeObject(action)}\n"); };


        public FlowState(IEnumerable<IFlowStateEffects> effects)
        {
            //TODO: START HERE! effects.ForEach()
        }


        //trigger action
        public void ResolveAction(FlowAction action)
        {
            _flowStateActions(action);
        }
        public void ResolveAction<T>(FlowAction<T> action)
        {
            _flowStateActions(action);
        }
        //Register various kinds of effects

        //TODO: remove this one?
        public void RegisterEffect(Predicate<FlowActionBase> filter, Func<FlowActionBase, FlowActionBase> effect)
        {
            _flowStateActions += (flowAction) =>
            {
                if (filter(flowAction))
                {
                    _flowStateActions(effect(flowAction));
                }
            };
        }

        public void RegisterEffect(FlowAction action, Func<FlowActionBase, FlowAction> effect)
        {
            _flowStateActions += (flowAction) =>
            {
                if (flowAction is FlowAction && onAction(action)((FlowAction)flowAction))
                {
                    _flowStateActions(effect(flowAction));
                }

            };
        }

        public void RegisterEffect<T>(FlowAction<T> action, Func<FlowActionBase, FlowAction<T>> effect)
        {
            _flowStateActions += (flowAction) =>
            {
                if (flowAction is FlowAction<T> && onAction(action)((FlowAction<T>)flowAction))
                {
                    _flowStateActions(effect(flowAction));
                }

            };
        }

        //selectors

        //state stuff?

        //consider: on(..actions).RegisterEffect(Func())

        //provided predicates?
        public static Predicate<FlowActionBase> onAction(params FlowActionBase[] actions)
        {
            return (flowAction) => actions.Any(a => a.Name == flowAction.Name);
        }

        public static bool IsResolvingAction<T>(FlowActionBase currentAction, FlowAction<T> matchingAction, out FlowAction<T> castAction)
        {
            if (onAction(matchingAction)(currentAction))
            {
                castAction = (FlowAction<T>)currentAction;
                return true;
            }

            castAction = matchingAction;
            return false;
        }

        public static FlowAction<T>? MatchingActionsResolving<T>(FlowActionBase currentAction, params FlowAction<T>[] matchingActions)
        {
            if (onAction(matchingActions)(currentAction))
            {
                return (FlowAction<T>)currentAction;
            }
            return null;
        }

        //public static Predicate<FlowAction<T>> onAction<T>(params FlowAction<T>[] actions)
        //{
        //    return (flowAction) => actions.Any(a => a.Name == flowAction.Name);
        //}
    }
}
