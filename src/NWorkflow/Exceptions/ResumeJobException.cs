using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Exceptions
{
    public class ResumeJobException : JobException
    {
        public ResumeJobException(string message, IJob job)
            : base(message, job)
        {
        }
    }
}
