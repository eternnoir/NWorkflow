using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow.Exceptions
{
    public class JobNameExistException:NWorkflowException
    {
        private IJob job;
        private IFlow flow;

        public JobNameExistException(IFlow Flow, IJob Job, string message)
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
