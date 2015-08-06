using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Exceptions
{
    public class JobException : NWorkflowException
    {
        private IJob job;
        public JobException(string message, IJob job):base(message)
        {
            this.job = job;
        }
    }
}
