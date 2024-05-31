using SessionStateFlow.package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package
{
    public class FlowStateData<T>: IFlowStateDataBase
    {
        private IFlowStateReducer<T> reducer;
        private T currentState;
        public FlowStateData(IFlowStateReducer<T> reducerDef)
        {
            reducer = reducerDef;

            currentState = reducer.InitialState;
        }

        public S CurrentState<S>(FlowDataSelector<T, S> selector)
        {
            return selector.selectorFunc(currentState);
        }

        public void FlowReduce(FlowActionBase action)
        {

            System.Diagnostics.Debug.Write($"\nReducing - PreviousState: {currentState}");
            currentState = reducer.Reduce(action, currentState);
            System.Diagnostics.Debug.WriteLine($" NewState: {currentState}");
        }
    }

    public interface IFlowStateDataBase
    {
        public void FlowReduce(FlowActionBase action);
    }
}
