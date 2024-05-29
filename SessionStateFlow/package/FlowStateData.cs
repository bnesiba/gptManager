using SessionStateFlow.package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package
{
    public class FlowStateData<T>
    {
        private IFlowStateReducer<T> reducer;
        private T currentState;
        public FlowStateData(FlowState state, IFlowStateReducer<T> reducerDef)
        {
            reducer = reducerDef;

            currentState = reducer.InitialState;
            //state._flowStateActions += (action) => currentState = reducer.Reduce(action, currentState);

            //TODO: DeleteMe
            state._flowStateActions += (action) => 
            {
                System.Diagnostics.Debug.Write($"Reducing - PreviousState: {currentState}");
                currentState = reducer.Reduce(action, currentState);
                System.Diagnostics.Debug.WriteLine($" NewState: {currentState}");
             };
        }

        public S CurrentState<S>(FlowDataSelector<T, S> selector)
        {
            return selector.selectorFunc(currentState);
        }
    }
}
