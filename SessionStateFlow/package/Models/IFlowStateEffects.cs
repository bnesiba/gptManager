using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package.Models
{
    public interface IFlowStateEffects
    {
        public List<IFlowEffectBase> SideEffects { get; }
    }

    public class FlowEffect : IFlowEffectBase
    {
        public List<FlowAction> TriggeringActions { get; init; }
        public Func<FlowAction, FlowActionBase> SideEffect { get; init;  }

        public FlowEffect(Func<FlowActionBase, FlowActionBase> sideEffect, params FlowAction[] triggeringActions)
        {
            TriggeringActions = triggeringActions.ToList();
            SideEffect = sideEffect;
        }
        public List<FlowActionBase> GetTriggeringActions()
        {
            return TriggeringActions.ToList<FlowActionBase>();
        }

        public Func<FlowActionBase, FlowActionBase> GetSideEffect()
        {
            Func<FlowActionBase, FlowActionBase> flowAct = (baseAction) =>
            {
                if (baseAction is FlowAction)
                {
                    return SideEffect((FlowAction)baseAction);
                }
                else
                {
                    return new FlowAction() { Name = "SIDE EFFECT TYPE ERROR!" };
                }
            };

            return flowAct;
        }
        //public Func<FlowActionBase, FlowActionBase> GetSideEffect()
        //{
        //    return (Func<FlowActionBase, FlowActionBase>)SideEffect;
        //}
    }

    public class FlowEffect<T> : IFlowEffectBase
    {
        public List<FlowAction<T>> TriggeringActions { get; init; }
        public Func<FlowAction<T>, FlowActionBase> SideEffect { get; init; }

        public FlowEffect(Func<FlowAction<T>, FlowActionBase> sideEffect, params FlowAction<T>[] triggeringActions)
        {
            TriggeringActions = triggeringActions.ToList();
            SideEffect = sideEffect;
        }

        public List<FlowActionBase> GetTriggeringActions()
        {
            return TriggeringActions.ToList<FlowActionBase>();
        }

        public Func<FlowActionBase, FlowActionBase> GetSideEffect()
        {
            Func<FlowActionBase, FlowActionBase> flowAct = (baseAction) =>
            {
                if (baseAction is FlowAction<T>)
                {
                    return SideEffect((FlowAction<T>)baseAction);
                }
                else
                {
                    return new FlowAction() { Name = "SIDE EFFECT TYPE ERROR!" };
                }
            };

            return flowAct;
        }
    }

    public interface IFlowEffectBase
    {
        public List<FlowActionBase> GetTriggeringActions();

        public Func<FlowActionBase, FlowActionBase> GetSideEffect();
    }

    public static class IFlowEffectBaseExtentions
    {

        public static FlowActionBase GetBaseActionFromDerived(FlowActionBase action)
        {
            return action;
        }

    }
}
