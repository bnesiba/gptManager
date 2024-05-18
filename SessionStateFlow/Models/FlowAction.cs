using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.Models
{
    public interface IFlowAction<T>: IFlowAction
    {
        public T? Parameters { get; init; }

    }

    public interface IFlowAction 
    {
        public string Name { get;}
    }
}
