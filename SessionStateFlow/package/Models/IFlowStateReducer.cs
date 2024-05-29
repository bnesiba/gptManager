using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package.Models
{
    public interface IFlowStateReducer<T>
    {
        public T InitialState { get; }

        public T Reduce(FlowActionBase action, T currentState);
    }
}
