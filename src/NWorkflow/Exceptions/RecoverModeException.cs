using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Exceptions
{
    public class RecoverModeException : NWorkflowException
    {
        public RecoverModeException(string message)
            : base(message)
        {

        }
    }
}
