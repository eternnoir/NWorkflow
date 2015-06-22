using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow.Exceptions
{
    public class NWorkflowException : Exception
    {
        public NWorkflowException(string message)
            : base(message)
        {
        }
    }
}
