using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow.Exceptions
{
    public class JobNotFoundException : NWorkflowException
    {
        private IJob job;
        private IFlow flow;

        public JobNotFoundException(IFlow flow, IJob job, string message)
            : base(message)
        {
            job = Job;
            flow = Flow;
        }

        public IJob Job
        {
            get
            {
                return job;
            }
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
