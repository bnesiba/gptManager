using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package.Models
{
    public interface IFlowStateEffects
    {
        public FlowActionBase SideEffects(FlowActionBase action);
    }
}
