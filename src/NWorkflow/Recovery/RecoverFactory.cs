using NWorkflow.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NWorkflow.Recovery
{
    public static class RecoverFactory
    {
        public static IRecover GetRecovery(RecoveryMode mode, IFlow flow)
        {
            IRecover ret = null;
            switch (mode)
            {
                case RecoveryMode.ONCE:
                    throw new NotImplementedException();
                    break;

                case RecoveryMode.STACK:
                    ret = new StackRecover(flow);
                    break;

                default:
                    throw new RecoverModeException("Not support recovery mode.");
                    
            }
            Debug.Assert(ret != null);
            return ret;
        }
    }

}
