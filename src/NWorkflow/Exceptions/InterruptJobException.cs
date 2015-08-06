using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Exceptions
{
    public class InterruptJobException : JobException
    {
        public InterruptJobException(string message, IJob job) :
            base(message, job)
        {
        }
    }
}
