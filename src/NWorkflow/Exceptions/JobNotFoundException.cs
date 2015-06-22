using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow.Exceptions
{
    public class JobNotFoundException : NWorkflowException
    {
        private IFlow flow;

        public JobNotFoundException(IFlow flow, string message)
            : base(message)
        {
            flow = Flow;
        }

        public IFlow Flow
        {
            get
            {
                return flow;
            }
        }
    }
}
